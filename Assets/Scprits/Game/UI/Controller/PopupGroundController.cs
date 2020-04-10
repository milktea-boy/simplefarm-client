using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 地块弹窗控制器
    /// </summary>
    public class PopupGroundController : BaseController<PopupGroundModel, PopupGroundView> {

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

    }
}