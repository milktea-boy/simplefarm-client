using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    /// <summary>
    /// 畜舍弹窗控制器二
    /// </summary>
    public class PopupLivestockController_2 : BaseController<PopupLivestockModel_2, PopupLivestockView_2>
    {
        public void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupLivestock_2_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupLivestock_2_ShowView", ShowView);
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
