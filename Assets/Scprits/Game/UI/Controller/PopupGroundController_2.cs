using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    /// <summary>
    /// 地块弹窗控制器二
    /// </summary>
    public class PopupGroundController_2 : BaseController<PopupGroundModel_2, PopupGroundView_2>
    {
        public void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupFarmland_2_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupFarmland_2_ShowView", ShowView);
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
