using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 猎人小屋弹窗视图
    /// </summary>
    public class PopupHunterView : BasePopup {

        private void Start()
        {
            gameObject.SetActive(false);
        }

    }
}
