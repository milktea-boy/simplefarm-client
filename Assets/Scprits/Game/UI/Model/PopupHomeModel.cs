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

        private void Awake()
        {
            SocketServer.GetSingleton().AddListener("global/farmDetailInfo", OnfarmDetailInfoCallback);
        }

        private void OnDestroy()
        {
            SocketServer.GetSingleton().RemoveListener("global/farmDetailInfo");
        }

        /// <summary>
        /// 获取农场详细信息
        /// </summary>
        public void GetFarmDetailInfo(Action<Hashtable> callback)
        {
            farmDetailInfoCallback = callback;
            SocketServer.GetSingleton().Send("global/farmDetailInfo", new object[] { });
        }

        private void OnfarmDetailInfoCallback(Hashtable data)
        {
            if (farmDetailInfoCallback != null)
            {
                farmDetailInfoCallback(data);
            }
        }
    }
}
