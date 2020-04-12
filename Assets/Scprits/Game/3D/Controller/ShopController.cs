using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 商店控制器
    /// </summary>
    public class ShopController : BaseController<ShopModel, ShopView> {

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_1", SetData);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_1", SetData);
        }

        public void SetData(object data)
        {

        }

    }
}