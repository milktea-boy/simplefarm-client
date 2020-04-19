using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameScene
{
    /// <summary>
    /// 初始化游戏数据模型
    /// </summary>
    public class initializationModel : MonoBehaviour
    {
        private Action<Hashtable> initializationCallback;

        private void Awake()
        {
            SocketServer.GetSingleton().AddListener("global/farmInfo",InitializationCallback);
        }

        private void OnDestroy()
        {
            SocketServer.GetSingleton().RemoveListener("global/farmInfo");
        }

        /// <summary>
        /// 初始化农场信息(刷新数据)
        /// </summary>
        /// <param name="callback"></param>
        public void Initialization(Action<Hashtable> callback)
        {
            initializationCallback = callback;

            SocketServer.GetSingleton().Send("global/farmInfo", new object[] { });
        }

        private void InitializationCallback(Hashtable data)
        {
            if (initializationCallback != null)
            {
                initializationCallback(data);
            }
        }

    }
}
