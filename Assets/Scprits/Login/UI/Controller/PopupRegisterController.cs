using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LoginScene
{
    /// <summary>
    /// 注册弹窗控制器
    /// </summary>
    public class PopupRegisterController : BaseController<PopupRegisterModel, PopupRegisterView>
    {

        public InputField inputUsername;
        public InputField inputPassword;
        public InputField inputSurePassword;
        public InputField inputNickname;

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupRegister_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupRegister_ShowView", ShowView);
        }

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

        public void OnBtnRegisterClick()
        {
            string username = inputUsername.text;
            string password = inputPassword.text;
            string surePassword = inputSurePassword.text;
            string nickname = inputNickname.text;

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
            if (surePassword == "")
            {
                PopupCommon.GetSingleton().ShowView("确认密码不能为空");
                return;
            }
            if (password != surePassword)
            {
                PopupCommon.GetSingleton().ShowView("两次密码不相同");
                return;
            }
            if (nickname == "")
            {
                PopupCommon.GetSingleton().ShowView("昵称不能为空");
                return;
            }

            Model.Register(username, password, nickname, RegisterCallback);
        }

        private void RegisterCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            PopupCommon.GetSingleton().ShowView("注册成功", null, false, () =>
            {
                View.HideView();
            });
        }

        private void ShowView(object data)
        {
            View.ShowView();
        }
    }
}