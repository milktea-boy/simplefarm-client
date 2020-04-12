using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 水井控制器
    /// </summary>
    public class WellController : BaseController<WellModel, WellView> {

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_7", SetData);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_7", SetData);
        }

        public void SetData(object data)
        {

        }

    }
}
