using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LoginScene
{
    /// <summary>
    /// 登录弹窗控制器
    /// </summary>
    public class PopupLoginController : BaseController<PopupLoginModel, PopupLoginView>
    {

        public InputField inputUsername;
        public InputField inputPassword;

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupLogin_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupLogin_ShowView", ShowView);
        }

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

        public void OnBtnLoginClick()
        {
            string username = inputUsername.text;
            string password = inputPassword.text;

            if (username == "")
            {
                PopupCommon.GetSingleton().ShowView("用户名不能为空");
                return;
            }
            if (password == "")
            {
                PopupCommon.GetSingleton().ShowView("密码不能为空");
                return;
            }

            Model.Login(username, password, LoginCallback);
        }

        private void LoginCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            PopupCommon.GetSingleton().ShowView("登录成功", null, false, () =>
            {
                SceneManager.LoadScene("Game");
            });
        }

        private void ShowView(object data)
        {
            View.ShowView();
        }

    }
}