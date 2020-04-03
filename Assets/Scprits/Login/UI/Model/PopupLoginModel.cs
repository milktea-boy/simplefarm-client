using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoginScene
{
    /// <summary>
    /// 登录弹窗模型
    /// </summary>
    public class PopupLoginModel : MonoBehaviour
    {

        private Action<Hashtable> loginCallback;

        private void Awake()
        {
            SocketServer.GetSingleton().AddListener("login/login", OnLoginCallback);
        }

        private void OnDestroy()
        {
            SocketServer.GetSingleton().RemoveListener("login/login");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="callback">完成回调</param>
        public void Login(string username, string password, Action<Hashtable> callback)
        {
            loginCallback = callback;
            SocketServer.GetSingleton().Send("login/login", new object[] { username, password });
        }

        private void OnLoginCallback(Hashtable data)
        {
            if (loginCallback != null)
                loginCallback(data);
        }
    }
}