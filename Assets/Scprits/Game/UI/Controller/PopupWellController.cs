using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 水井弹窗控制器
    /// </summary>
    public class PopupWellController : BaseController<PopupWellModel, PopupWellView> {

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

    }
}
