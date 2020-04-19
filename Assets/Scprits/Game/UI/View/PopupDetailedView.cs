using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    /// <summary>
    /// 游戏场景主菜视图
    /// </summary>
    public class PopupDetailedView : BasePopup
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
