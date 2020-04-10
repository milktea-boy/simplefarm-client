using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 畜舍弹窗控制器
    /// </summary>
    public class PopupLivestockController : BaseController<PopupLivestockModel, PopupLivestockView> {

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

    }
}
