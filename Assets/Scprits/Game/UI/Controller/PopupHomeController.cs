using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameScene {

    /// <summary>
    /// 家弹窗控制器
    /// </summary>
    public class PopupHomeController : BaseController<PopupHomeModel,PopupHomeView> {

        int farmLevel;//农场等级
        int curBuildingLevel;//当前建筑等级
        int curExp;//当前经验
        int upgradeNeedExp;//升级所需经验
        int curCoin;//当前金币
        int upPrice;//升级所需金币

        private void Awake()
        {
            base.Awake();

            Model.GetFarmInfo(OnfarmInfoCallback);

            Model.GetFarmDetailInfo(OnfarmDetailInfoCallback);

            Model.GetUpgradeInfo(0,OnUpgradeInfoCallback);
        }

        private void OnDestroy()
        {
            
        }

        /// <summary>
        /// 家建筑升级点击事件
        /// </summary>
        public void OnBtnUpgradeClick()
        {
            if (this.farmLevel > this.curBuildingLevel && this.curExp > this.upgradeNeedExp && this.curCoin > this.upPrice)
            {
                Model.Upgrade(0, OnUpgradeCallback);
            }
        }

        private void OnfarmInfoCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            string curLevel = data["buildInfo"].ToString();

            this.curBuildingLevel = int.Parse(curLevel);
            ArrayList buildInfo = data["buildInfo"] as ArrayList;
            for (int i = 0; i < buildInfo.Count; i++) {
                Hashtable args = buildInfo[0] as Hashtable;
                this.curBuildingLevel = int.Parse(args["level"].ToString());
            }
            //ArrayList buildInfoArrayList = data["buildInfo"]; //先跳过

        }

        private void OnfarmDetailInfoCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();
            string userID = data["userId"].ToString();
            string nickName = data["nickname"].ToString();
            string level = data["level"].ToString();
            string exp = data["exp"].ToString();
            string needExp = data["needExp"].ToString();
            string coin = data["coin"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            this.farmLevel = int.Parse(level);
            this.curExp = int.Parse(exp);
            this.upgradeNeedExp = int.Parse(needExp);
            this.curCoin = int.Parse(coin);
        }

        private void OnUpgradeInfoCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();
            string price = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            this.upPrice = int.Parse(price);
        }

        private void OnUpgradeCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            //升级完成后的后续逻辑
        }

        public void OnBtnCloseClick()
        {
            View.HideView();
        }

    }

}