using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 地块控制器一
    /// </summary>
    public class GroundController_1 : BaseController<GroundModel_1, GroundView_1> {

        int buildId;
        int buildLevel;
        int maxLevel;

        int playerLevel;
        int coin;
        string nickName;
        int playerExp;
        int playerNeedExp;

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_5", SetData);

            MessageManager.GetSingleton().RegisterMessageListener("OnTap_5", SendToPopupOverview);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_5", SetData);

            MessageManager.GetSingleton().UnRegisterMessageListener("OnTap_5", SendToPopupOverview);
        }

        public void SetData(object data)
        {
            object[] args = (object[])data;

            this.buildId = int.Parse(args[0].ToString());
            this.buildLevel = int.Parse(args[1].ToString());
            this.maxLevel = int.Parse(args[2].ToString());

            this.playerLevel = int.Parse(args[3].ToString());
            this.coin = int.Parse(args[4].ToString());
            this.nickName = args[5].ToString();
            this.playerExp = int.Parse(args[6].ToString());
            this.playerNeedExp = int.Parse(args[7].ToString());
        }

        void SendToPopupOverview(object data)
        {
            object[] args = (object[])data;

            int dataBuildId = int.Parse(args[0].ToString());

            Debug.Log(this.buildId);
            Debug.Log(dataBuildId);

            if (dataBuildId == this.buildId)
            {
                MessageManager.GetSingleton().SendMsg("PopupOverview_ShowView", new object[] { this.buildId, this.buildLevel, this.maxLevel, this.coin });//传输的值中，第一个是BuildID，第二个是是否显示建造按钮，第三个是是否显示升级按钮,第四个是当前玩家金币数
            }
        }

    }
}