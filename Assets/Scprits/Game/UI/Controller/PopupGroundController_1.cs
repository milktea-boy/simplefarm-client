using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 地块弹窗控制器一
    /// </summary>
    public class PopupGroundController_1 : BaseController<PopupGroundModel_1, PopupGroundView_1> {

        public void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupFarmland_1_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupFarmland_1_ShowView", ShowView);
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