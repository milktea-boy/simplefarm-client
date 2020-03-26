using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using Constant;
using MySocket;
using MyThread;
using MessageControlSpace;
using UnityEngine.SceneManagement;

public class SocketServer : MonoBehaviour
{

    public Dictionary<string, int> serverMessage;

    //让该类成为一个单例，方便整个工程对它的调用
    public static SocketServer socketServer = null;
    public static SocketServer GetSingleton()
    {
        return socketServer;
    }

    private CreateSocket MyCreateSocket;
    public Transmit MyTransmit;
    private MessageControlSpace.MessageControl MyMessageControl;
    public PostMessageThread MyPostMessageThread;

    private Thread _ConnectThread = null;

    /*控制game场景加载完成后，才响应某些消息*/
    private bool _isGameSceneLoadOk = false;
    public bool IsGameSceneLoadOk
    {
        get
        {
            return _isGameSceneLoadOk;
        }
        set
        {
            _isGameSceneLoadOk = value;
        }
    }

    public string ip = "127.0.0.1";
    public int port = 8888;

    void Awake()
    {
        if (socketServer == null)
        {
            socketServer = this;
        }

        Constants.IP = this.ip;
        Constants.PORT = this.port;

        Init();
    }
    public void Init() {

        MyCreateSocket = new CreateSocket();
        MyTransmit = new Transmit();
        MyMessageControl = new MessageControl();
        MyPostMessageThread = new PostMessageThread();

        MyMessageControl.MessageControlParaInit();
        MyPostMessageThread.PostMessageThreadParaInit();


        MyCreateSocket.CreateSocketGetPoint(MyMessageControl);
        MyTransmit.TransmitGetPoint(MyCreateSocket);
        MyPostMessageThread.PostMessageThreadGetPoint(MyMessageControl, MyTransmit);
    }

    public void ShowLog(string s)
    {
        Debug.Log(s);
    }

    public void ShowLogcat(string titleString, string content)
    {
        if (content.Length > 500)
        {
            double partCountF = content.Length / 500;
            int partCountI = (int)Math.Ceiling(partCountF);
            for (int i = 0; i <= partCountI; i++)
            {
                string stemp = "";
                if (i * 500 + 500 < content.Length)
                {
                    stemp = content.Substring(i * 500, 500);
                }
                else
                {
                    int remain = content.Length - i * 500;
                    stemp = content.Substring(i * 500, remain);
                }

                SocketServer.GetSingleton().ShowLog(titleString + " part " + i + " : " + stemp);
            }
        }
    }

    public Hashtable GetHashTable(object obj)
    {
        Dictionary<string, object> objs = (Dictionary<string, object>)obj as Dictionary<string, object>;

        Hashtable h = new Hashtable();

        foreach (KeyValuePair<string, object> kvp in objs)
        {
            SocketServer.GetSingleton().ShowLog(kvp.Key + " : " + kvp.Value);
            h.Add(kvp.Key, kvp.Value);
        }
        return h;
    }

    /*
     * 发送消息
     */
    public bool Send(string method, object[] args)
    {
        if (args == null) args = new object[] { };
            return MyCreateSocket._sendMsg(method, args);
    }
   
    public void ClearAllMessage() {
        MyPostMessageThread.ClearAllState();
    }

    void FixedUpdate()
    {
        if (MyPostMessageThread != null)
        {
            MyPostMessageThread.PostThread();
        }
    }

    public void AddListener(string name, Transmit.Callback call)
    {
        
        MyTransmit.AddEventListener(name, call);
    }

    public void RemoveListener(string name)
    {

        MyTransmit.RemoveEventListener(name);
    }


    void OnDestroy()
    {
        Debug.Log("socket close");
        if (MyCreateSocket != null)
        {
            MyCreateSocket.SocketClose();
        }
    }

    public void AddReviewMessage(string jsonStr,byte[] data)
    {
        MyCreateSocket.AddMessage(jsonStr, data);
    }

    public void ClearMessage()
    {
        MyCreateSocket.ClearMessage();
    }

    public void ConnectServer(string ip, int port)
    {
        Constants.IP = ip;
        Constants.PORT = port;

        MyCreateSocket = new CreateSocket();
    }

    public void DisConnect()
    {
        MyCreateSocket.SocketClose();
        MyCreateSocket = null;
    }

}