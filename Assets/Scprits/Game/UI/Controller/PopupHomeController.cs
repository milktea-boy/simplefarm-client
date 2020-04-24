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

        public Text textNickName;
        public Text textGoldCoins;
        public Text textGrade;
        public Text textTips;
        public Slider SliderExpBar;
        public Text textExpShow;

        private void Awake()
        {
            base.Awake();

            Model.GetFarmDetailInfo(OnfarmDetailInfoCallback);

            MessageManager.GetSingleton().RegisterMessageListener("PopupHome_ShowView",ShowView);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("PopupHome_ShowView",ShowView);
        }

        private void OnfarmDetailInfoCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();
            
            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            string userID = data["userId"].ToString();
            string nickName = data["nickname"].ToString();
            string level = data["level"].ToString();
            string exp = data["exp"].ToString();
            string needExp = data["needExp"].ToString();
            string coin = data["coin"].ToString();

            this.farmLevel = int.Parse(level);
            this.curExp = int.Parse(exp);
            this.upgradeNeedExp = int.Parse(needExp);
            this.curCoin = int.Parse(coin);

            //分配改显示的值给对应组件
            this.textNickName.text = nickName;
            this.textGoldCoins.text = coin;
            this.textGrade.text = string.Format("Level:{0}",level);
            if (int.Parse(level) < 3 && int.Parse(level) > 0)
            {
                this.textTips.text = string.Format("玩家需要{0}点经验升级到第{1}级！", needExp, (int.Parse(level) + 1).ToString());
            }
            else
            {
                this.textTips.text = string.Format("玩家已满级！");
            }

            this.SliderExpBar.minValue = 0;
            this.SliderExpBar.maxValue = upgradeNeedExp; ;
            this.SliderExpBar.value = curExp;

            this.textExpShow.text = string.Format("{0}/{1}",exp, needExp);        
        }

        public void OnBtnCloseClick()
        {
            View.HideView();

            MessageManager.GetSingleton().SendMsg("MainUI_ShowView");
        }

        private void ShowView(object data)
        {
            MessageManager.GetSingleton().SendMsg("MainUI_HideView");

            Model.GetFarmDetailInfo(OnfarmDetailInfoCallback);

            View.ShowView();
        }

    }

}