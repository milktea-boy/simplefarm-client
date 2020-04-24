﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 水井弹窗控制器
    /// </summary>
    public class PopupWellController : BaseController<PopupWellModel, PopupWellView> {
        public void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupWell_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupWell_ShowView", ShowView);
        }

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

        private void ShowView(object data)
        {
            View.ShowView();
        }

    }
}
