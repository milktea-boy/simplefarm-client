using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameScene
{
    /// <summary>
    /// 游戏场景主菜单控制器
    /// </summary>
    public class PopupDetailedController : BaseController<PopupDetailedModel, PopupDetailedView>
    {
        public Button btnUpgrade;

        public Button btnBuild;

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupDetailed_ShowView", OpenPopupDetailed);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().RegisterMessageListener("PopupDetailed_ShowView", OpenPopupDetailed);
        }

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

        void OpenPopupDetailed(object data)
        {
            object[] args = (object[])data;

            //根据data中的第二个参数判断是否显示 建造 案件
            if ((bool)args[1])
            {
                this.btnBuild.gameObject.SetActive(true);
            }
            else if(!(bool)args[1])
            {
                this.btnBuild.gameObject.SetActive(false);
            }

            //根据data中的第三个参数判断是否显示升级按钮
            if ((bool)args[2])
            {
                this.btnUpgrade.gameObject.SetActive(true);
            }
            else if (!(bool)args[2])
            {
                this.btnUpgrade.gameObject.SetActive(false);
            }

            View.ShowView();
        }
    }
}
