using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoginScene
{
    /// <summary>
    /// 登录场景主UI控制器
    /// </summary>
    public class LoginMainUIController : BaseController<LoginMainUIModel, LoginMainUIView>
    {

        public void OnBtnLoginClick()
        {
            MessageManager.GetSingleton().SendMsg("PopupLogin_ShowView");
        }

        public void OnBtnRegisterClick()
        {
            MessageManager.GetSingleton().SendMsg("PopupRegister_ShowView");
        }

    }
}