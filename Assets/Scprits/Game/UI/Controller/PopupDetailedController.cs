using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameScene
{
    /// <summary>
    /// 游戏场景详细菜单控制器
    /// </summary>
    public class PopupDetailedController : BaseController<PopupDetailedModel, PopupDetailedView>
    {
        public Button btnUpgrade;

        public Button btnBuild;

        public Button btnSelfMenu;

        //记录当前已点击建筑的 ID、等级和最大等级
        int buildId;

        int buildLevel;
        int maxLevel;

        int coin;//玩家当前金币数

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("PopupDetailed_ShowView", OpenPopupDetailed);
        }

        private void Start()
        {
            // 默认按钮不显示
            this.btnBuild.gameObject.SetActive(false);

            this.btnUpgrade.gameObject.SetActive(false);
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

            this.buildId = int.Parse(args[0].ToString());

            int buildLevel = int.Parse(args[1].ToString());

            int maxLevel = int.Parse(args[2].ToString());

            //根据data中的第二个参数判断是否显示 建造 案件
            if (buildLevel == 0)
            {
                this.btnBuild.gameObject.SetActive(true);
            }
            else if(buildLevel > 0)
            {
                this.btnBuild.gameObject.SetActive(false);
            }

            //根据data中的第三个参数判断是否显示升级按钮
            if (buildLevel < maxLevel)
            {
                this.btnUpgrade.gameObject.SetActive(true);
            }
            else
            {
                this.btnUpgrade.gameObject.SetActive(false);
            }

            //data中的第四个参数是玩家当前金币数
            this.coin = int.Parse(args[3].ToString());

            Debug.Log("玩家持有金额为： " + this.coin);

            View.ShowView();
        }

        public void OnBtnUpgradeClick()
        {
            Model.GetUpgradeBuildInfo(this.buildId, GetUpgradeBuildInfoCallback);

            Debug.Log("buildId: " + this.buildId);
        }

        private void GetUpgradeBuildInfoCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            int price = int.Parse(data["price"].ToString());//升级价格

            if (this.coin < price)
            {
                PopupCommon.GetSingleton().ShowView("金币不足无法升级");
                PopupCommon.GetSingleton().HideView();
            }
            else if (this.coin >= price)
            {
                Model.UpgradeBuild(this.buildId, UpgradeBuildCallback);
            }
                
        }

        private void UpgradeBuildCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            PopupCommon.GetSingleton().ShowView("升级成功", null, false, () =>
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("Refresh_GameScene");
            });
        }
    }
}
