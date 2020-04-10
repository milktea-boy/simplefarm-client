using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 商店弹窗控制器
    /// </summary>
    public class PopupShopController : BaseController<PopupShopModel, PopupShopView> {

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

    }
}