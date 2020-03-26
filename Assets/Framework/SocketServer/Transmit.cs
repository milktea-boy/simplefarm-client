using System.Collections;
using System.Collections.Generic;
using Constant;


namespace MySocket
{
    public class Transmit
    {
        public Transmit()
        {

        }

        private CreateSocket m_CreateSocket;

        public void TransmitGetPoint(CreateSocket MyCreateSocket)
        {
            m_CreateSocket = MyCreateSocket;
            callbacks = new Dictionary<string, Callback>();
        }

        public delegate void Callback(Hashtable message);
        private Dictionary<string, Callback> callbacks;

        /*add callback*/
        public void AddEventListener(string name, Callback call)
        {
            if (callbacks.ContainsKey(name) == false)
                callbacks.Add(name, call);
            else
                callbacks[name] = call;
        }
    
        public void RemoveEventListener(string name)
        {
            if (callbacks.ContainsKey(name))
            {
                callbacks.Remove(name);
            }
        }

        public void PostMsgControl(Message m_MyMessage)
        {
            Hashtable table = m_MyMessage.hashtable;
            string method = table["method"].ToString();

            Hashtable args = new Hashtable();

            args = table["data"] as Hashtable;
            Message myMessage = new Message();
            myMessage.hashtable = args;
            myMessage.data = m_MyMessage.data;
            if (method == "NetDown")
            {
                Constants.socketConnected = false;
                m_CreateSocket.CreateReceiveThread();
            }
            else if (method == "NetConnected")
            {
                Constants.socketConnected = true;
            }

            if (callbacks.ContainsKey(method))
            {
                Hashtable ht = myMessage.hashtable==null?new Hashtable():myMessage.hashtable;
                ht.Add("byteData", myMessage.data);
                callbacks[method](ht);
            }

        }

        /*从后台切回的过滤处理*/
        public void SelectPostMsgControl(Message m_MyMessage)
        {

        }
    }
}