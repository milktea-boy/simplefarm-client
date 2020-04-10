using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 地块弹窗视图
    /// </summary>
    public class PopupGroundView : BasePopup {

        private void Start()
        {
            gameObject.SetActive(false);
        }

    }
}