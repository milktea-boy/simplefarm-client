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
    public class PopupOverviewController : BaseController<PopupOverviewModel, PopupOverviewView>
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

            MessageManager.GetSingleton().RegisterMessageListener("PopupOverview_ShowView", OpenPopupOverview);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().RegisterMessageListener("PopupOverview_ShowView", OpenPopupOverview);
        }

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

        void OpenPopupOverview(object data)
        {
            object[] args = (object[])data;

            this.buildId = int.Parse(args[0].ToString());

            this.buildLevel = int.Parse(args[1].ToString());

            this.maxLevel = int.Parse(args[2].ToString());

            Debug.Log("当前建筑等级：" + this.buildLevel);
            Debug.Log("最大限等级：" + this.maxLevel);

            //根据data中的第二个参数判断是否显示 建造 按钮
            this.btnBuild.gameObject.SetActive(false);
            this.btnUpgrade.gameObject.SetActive(false);

            if (this.buildLevel == 0)
            {
                this.btnUpgrade.gameObject.SetActive(false);
                this.btnBuild.gameObject.SetActive(true);
            }
            else
            {
                if (this.buildLevel < this.maxLevel)
                {
                    this.btnUpgrade.gameObject.SetActive(true);
                    this.btnBuild.gameObject.SetActive(false);
                }
                else
                {
                    this.btnUpgrade.gameObject.SetActive(false);
                    this.btnBuild.gameObject.SetActive(false);
                }
            }

            //data中的第四个参数是玩家当前金币数
            this.coin = int.Parse(args[3].ToString());

            Debug.Log("玩家持有金额为： " + this.coin);

            View.ShowView();
        }

        public void OnBtnUpgradeClick()
        {
            if (this.buildLevel > 0 && this.buildLevel < this.maxLevel)
            {
                Model.GetUpgradeBuildInfo(this.buildId, GetUpgradeBuildInfoCallback);

                Debug.Log("buildId: " + this.buildId);
            }
            else
            {
                PopupCommon.GetSingleton().ShowView("无法升级！");
            }
        }

        public void OnBtnBuilding()
        {
            if (this.buildLevel == 0)
            {
                Model.GetUpgradeBuildInfo(this.buildId, GetUpgradeBuildInfoCallback);

                Debug.Log("buildId: " + this.buildId);
            }
            else
            {
                PopupCommon.GetSingleton().ShowView("建筑已建造！");
            }
        }

        public void OnBtnDetails()
        {
            Debug.Log("当前打开" + this.buildId + "的详细界面");
            if (this.buildId == 0)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupHome_ShowView");
            }
            else if (this.buildId == 1)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupShop_ShowView");
            }
            else if (this.buildId == 2)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupBarn_ShowView");
            }
            else if (this.buildId == 3)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupLivestock_1_ShowView");
            }
            else if (this.buildId == 4)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupLivestock_2_ShowView");
            }
            else if (this.buildId == 5)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupFarmland_1_ShowView");
            }
            else if (this.buildId == 6)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupFarmland_2_ShowView");
            }
            else if (this.buildId == 7)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupWell_ShowView");
            }
            else if (this.buildId == 8)
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("PopupHunterHouse_ShowView");
            }
        }

        //建造与升级共用的获取建造/升级信息方法
        private void GetUpgradeBuildInfoCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            int price = int.Parse(data["price"].ToString());//升级/建造价格

            if (this.buildLevel == 0)
            {
                if (this.coin < price)
                {
                    PopupCommon.GetSingleton().ShowView("金币不足无法建造");
                    PopupCommon.GetSingleton().HideView();
                }
                else if (this.coin >= price)
                {
                    Model.UpgradeBuild(this.buildId, BuildingCallback);
                }
            }
            else
            {
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

        private void BuildingCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            MessageManager.GetSingleton().SendMsg("Building_" + this.buildId,new object[] {this.buildId,this.buildLevel});

            PopupCommon.GetSingleton().ShowView("建造成功", null, false, () =>
            {
                View.HideView();

                MessageManager.GetSingleton().SendMsg("Refresh_GameScene");
            });
        }
    }
}
