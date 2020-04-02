using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoginScene
{
    /// <summary>
    /// 注册弹窗视图
    /// </summary>
    public class PopupRegisterView : BasePopup
    {

        private void Start()
        {
            // 默认弹窗不显示
            gameObject.SetActive(false);
        }

    }
}