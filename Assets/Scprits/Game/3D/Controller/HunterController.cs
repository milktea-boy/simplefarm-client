using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 猎人小屋控制器
    /// </summary>
    public class HunterController : BaseController<HunterModel, HunterView> {

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_8", SetData);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_8", SetData);
        }

        public void SetData(object data)
        {

        }

    }
}
