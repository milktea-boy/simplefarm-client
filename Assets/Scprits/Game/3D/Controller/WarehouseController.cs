using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 仓库控制器
    /// </summary>
    public class WarehouseController : BaseController<WarehouseModel, WarehouseView> {

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_2", SetData);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_2", SetData);
        }

        public void SetData(object data)
        {

        }

    }
}
