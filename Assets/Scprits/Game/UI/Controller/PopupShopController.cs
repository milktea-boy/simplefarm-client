using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 商店弹窗控制器
    /// </summary>
    public class PopupShopController : BaseController<PopupShopModel, PopupShopView> {
        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupShop_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupShop_ShowView", ShowView);
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