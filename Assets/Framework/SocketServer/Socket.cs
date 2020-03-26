using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Constant;
using System.Runtime.InteropServices;

namespace MySocket
{
    public class CreateSocket
    {
        public CreateSocket()
        {
            CreateReceiveThread();

            CreateListenerThread();
        }
        //网络监听线程的ID
        private Thread m_nListenerThread = null;

        //创建网络监听线程
        private void CreateListenerThread()
        {
            //如果之前存在网络监听线程，那么先将监听线程关闭
            try
            {
                if (m_nListenerThread != null)
                {
                    m_nListenerThread.Abort();

                    m_nListenerThread = null;
                }
            }
            catch (System.Exception ex)
            {
                SocketServer.GetSingleton().ShowLog("ListenerThread:" + ex.Message);
            }

            m_nListenerThread = new Thread(new ThreadStart(socketListener));

            m_nListenerThread.Start();
        }


        private MessageControlSpace.MessageControl m_MyMessageControl;

        public void CreateSocketGetPoint(MessageControlSpace.MessageControl MyMessageControl)
        {
            this.m_MyMessageControl = MyMessageControl;
        }

        public Socket MySocket;

        //接收线程的ID
        private Thread m_nReceiveThread = null;

        //可以重连的有效次数
        private int m_nRelineCount = 0;

        public int GetRelineCount()
        {
            return m_nRelineCount;
        }

        //网络是否连接上的标志
        private bool m_nSocketStartFlag;

        public bool GetSocketStartFlag()
        {
            return m_nSocketStartFlag;
        }

        //-----------------------------------
        //网络模块的创建和网络数据的读取
        //-----------------------------------

        //创建网络接收线程
        public void CreateReceiveThread()
        {
            //之前如果存在接收线程，则将接收线程关闭，重新创建新的接收线程
            try
            {
                if (m_nReceiveThread != null)
                {
                    SocketServer.GetSingleton().ShowLog("CreateReceiveThread");
                    m_nReceiveThread.Abort();
                    m_nReceiveThread = null;
                }
            }
            catch (System.Exception ex)
            {
                SocketServer.GetSingleton().ShowLog("ReceiveThread" + ex.Message);
            }

            m_nReceiveThread = new Thread(new ThreadStart(SocketConnect));

            m_nReceiveThread.Start();
        }

        //程序退出时，socket的释放
        //程序退出时，socket的释放
        public void SocketClose()
        {
            if (MySocket != null)
            {
                MySocket.Close();
                MySocket = null;
            }

            try
            {
                m_nReceiveThread.Abort();
                m_nReceiveThread = null;
            }
            catch (System.Exception ex)
            {
                SocketServer.GetSingleton().ShowLog("ReceiveThread:" + ex.Message);
            }

            try
            {
                m_nListenerThread.Abort();
                m_nListenerThread = null;
            }
            catch (System.Exception ex)
            {
                SocketServer.GetSingleton().ShowLog("ListenerThread:" + ex.Message);
            }
        }



        //主调用接口：创建网络模块，用户网络模块的建立
        public void SocketConnect()
        {
            Thread.Sleep(100);

            m_nSocketStartFlag = false;

            m_nSocketStartFlag = _socketStart();

            if (!m_nSocketStartFlag)
            {
                return;
            }

            //网络连接成功向服务器发送公钥，请求本次的AES加密密钥
            //SendPublicKey();
            //连接建立成功，开始进行数据的接收
            _myReceiveControl();
        }

        [DllImport("__Internal")]
        private static extern string getIPv6(string mHost, string mPort);

        public static string GetIPv6(string mHost, string mPort)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
			string mIPv6 = getIPv6(mHost, mPort);
			return mIPv6;
#else
            return mHost + "&&ipv4";
#endif
        }

        void getIPType(String serverIp, String serverPorts, out String newServerIp, out AddressFamily mIPType)
        {
            mIPType = AddressFamily.InterNetwork;
            newServerIp = serverIp;
            try
            {
                string mIPv6 = GetIPv6(serverIp, serverPorts);
                if (!string.IsNullOrEmpty(mIPv6))
                {
                    string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
                    if (m_StrTemp != null && m_StrTemp.Length >= 2)
                    {
                        string IPType = m_StrTemp[1];
                        if (IPType == "ipv6")
                        {
                            newServerIp = m_StrTemp[0];
                            mIPType = AddressFamily.InterNetworkV6;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SocketServer.GetSingleton().ShowLog("GetIPv6 error:" + e);
            }
        }



        //客户端和服务器建立链接
        private bool _socketStart()
        {
            String serverIp = Constants.IP;
            String serverPorts = Constants.PORT.ToString();

            String newServerIp = "";
            AddressFamily newAddressFamily = AddressFamily.InterNetwork;
            getIPType(serverIp, serverPorts, out newServerIp, out newAddressFamily);
            if (!string.IsNullOrEmpty(newServerIp))
            {
                serverIp = newServerIp;
            }

            MySocket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint IPAndPort = new IPEndPoint(IPAddress.Parse(serverIp), Constants.PORT);

            SocketServer.GetSingleton().ShowLog("connect: " + serverIp + ":" + Constants.PORT);

            try
            {
                MySocket.Connect(IPAndPort);

                //建立好连接，将断线重连计数置为0
                m_nRelineCount = 0;

                //建立好连接，开始进行心跳监控和心跳计时
                m_recriveHeartTime = _getCurTime();
                m_sendHeartTime = _getCurTime();

                m_nSocketStartFlag = true;

                _netConnectedControl();

                return true;
            }
            catch (SocketException e)
            {
                SocketServer.GetSingleton().ShowLog("connect:" + e.Message);

                m_nSocketStartFlag = false; 

                //网络连接不上，断线处理
                _netDownControl();

                return false;
            }
        }
        public static Int64[] ping = new Int64[2];

        private byte[] head = new byte[4];
        private int messageLenth;

        List<byte> byteList = new List<byte>();

        //在套接字上进行信息的接收处理:数据信息的组包、解包
        private void _myReceiveControl()
        {
            //从socket接收到的数据缓存
            int nRecLenth = 0;
            byte[] Buff;

            while (true)
            {
                Buff = new byte[102400];

                try
                {
                    nRecLenth = MySocket.Receive(Buff);
                }
                catch (System.Exception e)
                {
                    //客户端断网
                    SocketServer.GetSingleton().ShowLog("Receive:" + e.Message);

                    _netDownControl();

                    break;
                }

                if (nRecLenth <= 0)
                {
                    SocketServer.GetSingleton().ShowLog("nRecLenth <= 0  == " + nRecLenth);

                    //服务器程序关闭
                    _netDownControl();

                    break;
                }

                byte[] bytes = new byte[nRecLenth];
                Buffer.BlockCopy(Buff, 0, bytes, 0, nRecLenth);

                byteList.AddRange(bytes);

                while (byteList.Count > 4 + 2)
                {
                    head[0] = byteList[3];
                    head[1] = byteList[2];
                    head[2] = byteList[1];
                    head[3] = byteList[0];

                    messageLenth = BitConverter.ToInt32(head, 0);

                    if (byteList.Count < messageLenth)
                    {
                        break;
                    }
                    else if (byteList.Count > messageLenth)
                    {
                        _receiveNetMessage(byteList.GetRange(0, messageLenth).ToArray());
                        byteList.RemoveRange(0, messageLenth);
                    }
                    else if (byteList.Count == messageLenth)
                    {
                        _receiveNetMessage(byteList.ToArray());
                        byteList.Clear();
                        break;
                    }
                }
            }
        }

        StringBuilder jsonStr;

        private void _receiveNetMessage(byte[] msgBytes)
        {
            byte[] typeBytes = new byte[2]; 
            typeBytes[0] = msgBytes[5];
            typeBytes[1] = msgBytes[4];
            short type= BitConverter.ToInt16(typeBytes, 0);

            int dataLength = msgBytes.Length - 4 - 2;

            jsonStr = new StringBuilder();
            jsonStr.Append(Encoding.UTF8.GetString(msgBytes, 4 + 2, dataLength));
            byte[] data = new byte[dataLength];
            Buffer.BlockCopy(msgBytes, msgBytes.Length - dataLength, data, 0, dataLength);
            switch (type)
            {
                case 99:
                    {
                        m_recriveHeartTime = _getCurTime();
                        if (jsonStr.Equals("-"))
                        {
                            _sendHeart("+");
                        }
                        break;
                    }
                case 0:
                    {
                        SocketServer.GetSingleton().ShowLog("recrive:"+jsonStr.ToString());
                        AddMessage(jsonStr.ToString(),data);
                        break;
                    }
            }

        }
        
        public void AddMessage(string jsonStr,byte[] data)
        {
            if (jsonStr.StartsWith("{") && jsonStr.EndsWith("}"))
            {
                Hashtable table = new Hashtable();

                table = TinyJSON.jsonDecode(jsonStr) as Hashtable;

                Message myMessage = new Message();
                myMessage.hashtable = table;
                myMessage.data = data;
                m_MyMessageControl.AddMessage(myMessage);
            }
        }

        public void ClearMessage()
        {
            m_MyMessageControl.ClearMessage();
        }



        //----------------------------------------------
        //客户端的网络监控，15秒没有接收到心跳，主动重连
        //---------------------------------------------
        private long m_recriveHeartTime = 0;
        private long m_sendHeartTime = 0;

        private long _getCurTime()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = DateTime.Now;
            return (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
        }

        public void socketListener()
        {
            Thread.Sleep(5000);

            long lCurTickTime = 0;

            while (true)
            {
                lCurTickTime = _getCurTime();

                //15秒没有接收到网络心跳
                if (lCurTickTime - m_recriveHeartTime > 600000)
                {
                    //将网络接收线程停止
                    try
                    {
                        m_nReceiveThread.Abort();

                        m_nReceiveThread = null;
                    }
                    catch (System.Exception ex)
                    {
                        SocketServer.GetSingleton().ShowLog("ReceiveThread:" + ex.Message);
                    }

                    //通知界面进行重连
                    _netDownControl();

                    m_recriveHeartTime = lCurTickTime;

                    continue;
                }

                //每隔5秒主动向服务器发送心跳
                if (lCurTickTime - m_sendHeartTime >= 5000 && MySocket != null)
                {
                    _sendHeart("@");

                    m_sendHeartTime = lCurTickTime;
                }

                Thread.Sleep(5000);
            }
        }

        //--------------------------------
        //发送函数的处理
        //--------------------------------

        /*发送的函数接口,最终发送的是密文*/
        public bool _sendMsg(string strMethod, object[] args)
        {
            Hashtable msgContext = new Hashtable();
            msgContext.Add("method", strMethod);
            msgContext.Add("data", args);

            string s = ReadFileTool.JsonByObject(msgContext);
            SocketServer.GetSingleton().ShowLog("send :" + s);
            byte[] buf = Encoding.UTF8.GetBytes(s);

            List<byte> bytes = new List<byte>();
            byte[] messageBodyLength = BitConverter.GetBytes(4 + 2 + buf.Length);
            Array.Reverse(messageBodyLength);
            bytes.AddRange(messageBodyLength);

            short messageType = 0;
            byte[] mT = BitConverter.GetBytes(messageType);
            Array.Reverse(mT);
            bytes.AddRange(mT);

            bytes.AddRange(buf);

            return _send(bytes.ToArray());
        }

        public bool _sendHeart(string s)
        {
            byte[] heart = Encoding.UTF8.GetBytes(s);

            List<byte> bytes = new List<byte>();
            byte[] messageBodyLenth = BitConverter.GetBytes(4 + 2 + heart.Length);
            Array.Reverse(messageBodyLenth);
            bytes.AddRange(messageBodyLenth);

            short messageType = 99;
            byte[] mT = BitConverter.GetBytes(messageType);
            Array.Reverse(mT);
            bytes.AddRange(mT);

            bytes.AddRange(heart);

            return _send(bytes.ToArray());
        }

        private bool _send(byte[] msg)
        {

            //需要发送的数据
            byte[] byteData = msg;
            byte[] sendData = new byte[byteData.Length];

            try
            {
                int nSendLenth = 0;
                if (MySocket != null)
                {
                    nSendLenth = MySocket.Send(msg);
                }

                if (nSendLenth == sendData.Length)
                {
                    return true;
                }


                return false;
            }
            catch (Exception e)
            {
                SocketServer.GetSingleton().ShowLog(e.ToString());

                return false;
            }
        }

        private void _netConnectedControl()
        {
            Hashtable table = new Hashtable();
            table.Add("method", "NetConnected");
            table.Add("data", new object[] { });
            Message m_MyMessage = new Message();
            m_MyMessage.hashtable = table;

            m_MyMessageControl.AddMessage(m_MyMessage);
        }

        //--------------------------
        //网络模块异常的处理
        //--------------------------

        //-----------------------------------
        //断网之后的处理
        //原则：断网之后，将网络状态全部清除
        //-----------------------------------
        private void _netDownControl()
        {
            //将目前的套接字关闭
            if (MySocket != null)
            {
                MySocket.Close();
                MySocket = null;
            }

            //将加密密钥重置，密钥再次连接之后重新获取
            //m_DataEncrypt.KeyReset();

            //通知界面掉线了
            m_nRelineCount++;
            _tellUINetDown();
        }

        //网络状态不好，断开连接.
        //此处的信息未加密，直接添加到通信队列中
        private void _tellUINetDown()
        {
            Hashtable table = new Hashtable();
            table.Add("method", "NetDown");
            table.Add("data", new object[] { });
            Message m_MyMessage = new Message();
            m_MyMessage.hashtable = table;
            m_MyMessageControl.AddMessage(m_MyMessage);
        }

    }
}