using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 仓库弹窗控制器
    /// </summary>
    public class PopupWarehouseController : BaseController<PopupWarehouseModel, PopupWarehouseView> {

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

    }
}