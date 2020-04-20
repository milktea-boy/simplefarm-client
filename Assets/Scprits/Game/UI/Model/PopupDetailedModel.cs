using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace GameScene
{
    /// <summary>
    /// 游戏场景详细菜单模型
    /// </summary>
    public class PopupDetailedModel : MonoBehaviour
    {
        private Action<Hashtable> getUpgradeBuildInfoCallback;

        private Action<Hashtable> upgradeBuildCallback;

        private void Awake()
        {
            SocketServer.GetSingleton().AddListener("global/upgradeBuildInfo", GetUpgradeBuildInfoCallback);

            SocketServer.GetSingleton().AddListener("global/upgradeBuild", UpgradeBuildCallback);
        }

        private void OnDestroy()
        {
            SocketServer.GetSingleton().RemoveListener("global/upgradeBuildInfo");

            SocketServer.GetSingleton().RemoveListener("global/upgradeBuild");
        }

        public void GetUpgradeBuildInfo(int buildId,Action<Hashtable> callback)
        {
            getUpgradeBuildInfoCallback = callback;

            SocketServer.GetSingleton().Send("global/upgradeBuildInfo", new object[]{ buildId });
        }

        public void UpgradeBuild(int buildId, Action<Hashtable> callback)
        {
            upgradeBuildCallback = callback;

            SocketServer.GetSingleton().Send("global/upgradeBuild", new object[] { buildId });
        }

        private void GetUpgradeBuildInfoCallback(Hashtable data)
        {
            if (getUpgradeBuildInfoCallback != null)
            {
                getUpgradeBuildInfoCallback(data);
            }
        }

        private void UpgradeBuildCallback(Hashtable data)
        {
            if (upgradeBuildCallback != null)
            {
                upgradeBuildCallback(data);
            }
        }
    }
}
