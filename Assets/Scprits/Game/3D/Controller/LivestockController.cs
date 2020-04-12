using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 畜舍控制器
    /// </summary>
    public class LivestockController : BaseController<LivestockModel, LivestockView> {

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_3", SetData);
            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_4", SetData);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_3", SetData);
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_4", SetData);
        }

        public void SetData(object data)
        {

        }

    }
}
