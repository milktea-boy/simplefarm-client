using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 家弹窗控制器
    /// </summary>
    public class PopupHomeController : BaseController<PopupHomeModel,PopupHomeView> {

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

    }

}