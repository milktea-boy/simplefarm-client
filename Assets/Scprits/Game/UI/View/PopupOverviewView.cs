using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    /// <summary>
    /// 游戏场景详细菜单视图
    /// </summary>
    public class PopupOverviewView : BasePopup
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
