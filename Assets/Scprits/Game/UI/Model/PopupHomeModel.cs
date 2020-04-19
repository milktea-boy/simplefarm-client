using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 家弹窗模型
    /// </summary>
    public class PopupHomeModel : MonoBehaviour {

        private Action<Hashtable> farmDetailInfoCallback;
        private Action<Hashtable> upgradeCallback;
        private Action<Hashtable> upgradeInfoCallback;

        private void Awake()
        {
            SocketServer.GetSingleton().AddListener("global/farmDetailInfo",OnUpgradeCallback);
            SocketServer.GetSingleton().AddListener("global/upgradeBuildInfo", OnUpgradeCallback);
            SocketServer.GetSingleton().AddListener("global/upgradeBuild", OnUpgradeCallback);
        }

        private void OnDestroy()
        {
            SocketServer.GetSingleton().RemoveListener("global/farmDetailInfo");
            SocketServer.GetSingleton().RemoveListener("global/upgradeBuildInfo");
            SocketServer.GetSingleton().RemoveListener("global/upgradeBuild");
        }

        /// <summary>
        /// 获取农场详细信息
        /// </summary>
        public void GetFarmDetailInfo(Action<Hashtable> callback)
        {
            farmDetailInfoCallback = callback;
            SocketServer.GetSingleton().Send("global/farmDetailInfo", new object[] { });
        }

        /// <summary>
        /// 获取建筑升级信息
        /// </summary>
        /// <param name="buildID">建筑ID</param>
        /// <param name="callback">完成回调</param>
        public void GetUpgradeInfo(int buildID,Action<Hashtable> callback)
        {
            upgradeInfoCallback = callback;
            SocketServer.GetSingleton().Send("global/upgradeBuildInfo", new object[] { buildID});
        }

        /// <summary>
        /// 升级建筑
        /// </summary>
        /// <param name="buildID">建筑ID</param>
        /// <param name="callback">完成回调</param>
        public void Upgrade(int buildID,Action<Hashtable> callback)
        {
            upgradeCallback = callback;
            SocketServer.GetSingleton().Send("global/upgradeBuild",new object[] { buildID});
        }

        private void OnfarmDetailInfoCallback(Hashtable data)
        {
            if (farmDetailInfoCallback != null)
            {
                farmDetailInfoCallback(data);
            }
        }

        private void OnUpgradeCallback(Hashtable data)
        {
            if (upgradeCallback != null)
            {
                upgradeCallback(data);
            }
        }

        private void OnUpgradeInfoCallback(Hashtable data)
        {
            if (upgradeInfoCallback != null)
            {
                upgradeInfoCallback(data);
            }
        }

    }
}
