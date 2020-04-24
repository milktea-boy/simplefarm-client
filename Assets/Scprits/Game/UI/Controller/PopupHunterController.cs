using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 猎人小屋控制器
    /// </summary>
    public class PopupHunterController : BaseController<PopupHunterModel, PopupHunterView> {
        public void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupHunterHouse_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupHunterHouse_ShowView", ShowView);
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