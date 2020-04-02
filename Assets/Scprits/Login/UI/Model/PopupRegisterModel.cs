using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoginScene
{
    /// <summary>
    /// 注册弹窗模型
    /// </summary>
    public class PopupRegisterModel : MonoBehaviour
    {
        private Action<Hashtable> registerCallback;

        private void Awake()
        {
            SocketServer.GetSingleton().AddListener("login/register", RegisterCallback);
        }

        private void OnDestroy()
        {
            SocketServer.GetSingleton().RemoveListener("login/register");
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="nickname">昵称</param>
        /// <param name="callback">完成回调</param>
        public void Register(string username, string password, string nickname, Action<Hashtable> callback)
        {
            registerCallback = callback;

            SocketServer.GetSingleton().Send("login/register", new object[] { username, password, nickname });
        }

        private void RegisterCallback(Hashtable data)
        {
            registerCallback(data);
        }
    }
}