using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{

    /// <summary>
    /// 畜舍弹窗控制器一
    /// </summary>
    public class PopupLivestockController_1 : BaseController<PopupLivestockModel_1, PopupLivestockView_1>
    {

        public void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupLivestock_1_ShowView", ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupLivestock_1_ShowView", ShowView);
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
