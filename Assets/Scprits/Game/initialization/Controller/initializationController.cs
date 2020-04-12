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
        }

        private void OnDestroy()
        {

        }

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
            //(string message, System.Object param = null)
            //MessageManager.GetSingleton().SendMsg 第二个参数是任何数据类型，可以分配给其他MVC
            //分开发送，细化处理
            //MessageManager.GetSingleton().SendMsg("Initialization_DataDistribution");
            int level = int.Parse(data["level"].ToString());//农场等级
            int coin = int.Parse(data["coin"].ToString());//金币数
            ArrayList buildinfo = data["buildInfo"] as ArrayList;//建筑信息 - 建筑ID、建筑等级0为未建立、当前农场最大等级

            for (int i = 0; i < buildinfo.Count; i++)
            {
                Hashtable args = buildinfo[i] as Hashtable;

                //int buildId;//建筑ID
                int buildLevel = 0;//建筑等级
                int maxLevel = 0;//当前农场等级的最大等级

                if (int.Parse(args["buildId"].ToString()) == i)
                    buildLevel = int.Parse(args["level"].ToString());
                maxLevel = int.Parse(args["maxLevel"].ToString());
                MessageManager.GetSingleton().SendMsg("Initialization_buildInfo_" + i, new object[] { buildLevel, maxLevel });

                int wellWaterCount = int.Parse(data["wellWaterCount"].ToString());//水井水数量，水井未建时为0
                //global/farmInfo 接口参数后续可能会改，先获取以上信
            }
        }

    }
}
