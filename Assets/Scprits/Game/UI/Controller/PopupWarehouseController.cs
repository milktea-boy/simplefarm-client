using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 仓库弹窗控制器
    /// </summary>
    public class PopupWarehouseController : BaseController<PopupWarehouseModel, PopupWarehouseView> {
        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupBarn_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupBarn_ShowView", ShowView);
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