using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    /// <summary>
    /// 初始化游戏数据控制器
    /// 进入Game场景获取后端游戏数据后分发给个模块
    /// </summary>
    public class initializationController : BaseController<initializationModel, initializationView>
    {
        private void Awake()
        {
            base.Awake();

            Model.Initialization(InitializationCallback);

            MessageManager.GetSingleton().RegisterMessageListener("Refresh_GameScene", RefreshGameScene);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Refresh_GameScene", RefreshGameScene);
        }

        /// <summary>
        /// 刷新场景的回调
        /// 等于以下方法要用服务器传回的数据告刷新景内所有的逻辑物体
        /// </summary>
        /// <param name="data"></param>
        private void InitializationCallback(Hashtable data)
        {
            bool success = bool.Parse(data["success"].ToString());
            string message = data["message"].ToString();

            if (!success)
            {
                PopupCommon.GetSingleton().ShowView(message);
                return;
            }

            //数据分发
            Debug.Log("开始数据分发----------------------------------------------");
            //(string message, System.Object param = null)
            //MessageManager.GetSingleton().SendMsg 第二个参数是任何数据类型，可以分配给其他MVC
            //分开发送，细化处理
            int level = int.Parse(data["level"].ToString());//农场等级

            int coin = int.Parse(data["coin"].ToString());//金币数

            string nickName = data["nickname"].ToString();//昵称

            int exp = int.Parse(data["exp"].ToString());//经验

            int needExp = int.Parse(data["needExp"].ToString());//升级所需经验

            Debug.Log("开始分发MainUI信息----------------------------------------------");

            //分发mainUI信息
            MessageManager.GetSingleton().SendMsg("Initialization_MainUIInfo", new object[] { level,coin,nickName,exp,needExp});

            Debug.Log("开始分发buildInfo----------------------------------------------");

            //分发buildInfo信息
            ArrayList buildInfo = data["buildInfo"] as ArrayList;//建筑信息 - 建筑ID、建筑等级0为未建立、当前农场最大等级

            Debug.Log("buildInfo的个数是：" + buildInfo.Count);

            foreach (var item in buildInfo)
            {
                Hashtable args = item as Hashtable;

                for (int i = 0; i < buildInfo.Count; i++)
                {
                    if (int.Parse(args["buildId"].ToString()) == i)
                    {
                        int buildId = int.Parse(args["buildId"].ToString());//建筑ID
                        int buildLevel = int.Parse(args["level"].ToString());//建筑等级
                        int maxLevel = int.Parse(args["maxLevel"].ToString());//当前农场等级的最大等级
                        MessageManager.GetSingleton().SendMsg("Initialization_buildInfo_" + i, new object[] { buildId, buildLevel, maxLevel, level,coin,nickName,exp,needExp});
                                                                                                           //建筑ID    建筑等级    建筑最大级 主等级  金币  昵称  经验  所需经验  
                    }
                }
            }

            /*
            int wellWaterCount = int.Parse(data["wellWaterCount"].ToString());//水井水数量，水井未建时为0

            ArrayList liveStockInfo = data["livestockInfo"] as ArrayList;

            ArrayList groundInfo = data["groundInfo"] as ArrayList;

            int hunterState = int.Parse(data["hunterState"].ToString());//猎人小屋状态，0-未派出，1-已派出，2-可收获，当猎人小屋已创建时存在
            */
        }

        private void RefreshGameScene(object data)
        {
            Model.Initialization(InitializationCallback);
        }

    }
}
