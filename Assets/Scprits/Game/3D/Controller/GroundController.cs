using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 地块控制器
    /// </summary>
    public class GroundController : BaseController<GroundModel, GroundView> {

        private void Awake()
        {
            base.Awake();

            //MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_5", SetData);
            //MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_6", SetData);
        }

        private void OnDestroy()
        {
            //MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_5", SetData);
            //MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_6", SetData);
        }

        public void SetData(object data)
        {

        }

    }
}