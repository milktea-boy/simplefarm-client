using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {

    /// <summary>
    /// 家控制器
    /// </summary>
    public class HomeController : BaseController<HomeModel, HomeView> {

        private void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_buildInfo_0", SetData);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_buildInfo_0", SetData);
        }

        public void SetData(object data)
        {

        }

    }
}