﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 猎人小屋控制器
    /// </summary>
    public class PopupHunterController : BaseController<PopupHomeModel, PopupHunterView> {

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

    }
}