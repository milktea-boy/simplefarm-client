using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 猎人小屋控制器
    /// </summary>
    public class HunterController : BaseController<HunterModel, HunterView> {

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

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_8", SetData);

            MessageManager.GetSingleton().RegisterMessageListener("OnTap_8", SendToPopupDetailed);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_8", SetData);

            MessageManager.GetSingleton().UnRegisterMessageListener("OnTap_8", SendToPopupDetailed);
        }

        void SetData(object data)
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

        void SendToPopupDetailed(object data)
        {
            object[] args = (object[])data;

            int dataBuildId = int.Parse(args[0].ToString());

            Debug.Log(this.buildId);
            Debug.Log(dataBuildId);

            if (dataBuildId == this.buildId)
            {
                MessageManager.GetSingleton().SendMsg("PopupDetailed_ShowView", new object[] { this.buildId, this.buildLevel, this.maxLevel, this.coin });//传输的值中，第一个是BuildID，第二个是是否显示建造按钮，第三个是是否显示升级按钮,第四个是当前玩家金币数
            }
        }

        /// <summary>
        /// 判断是否在详细界面显示建造按钮
        /// </summary>
        /// <param name="buildLevel"></param>
        /// <returns></returns>
        bool isBtnBuild(int buildLevel)
        {
            if (buildLevel == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否在详细界面显示升级按钮
        /// </summary>
        /// <param name="buildLevel"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        bool isBtnUpgrade(int buildLevel, int maxLevel)
        {
            if (buildLevel < maxLevel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
