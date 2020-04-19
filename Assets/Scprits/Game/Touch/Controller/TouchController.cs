using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

namespace GameScene
{
    /// <summary>
    /// 触碰控制器
    /// 此处控制游戏场景中可点击3D物体的点击事件
    /// </summary>
    public class TouchController : MonoBehaviour
    {
        private int curBuildingNum;

       public void OnTap(Gesture gesture)
        {
            Debug.Log("当前Tap建筑的序号是：" + gesture.pickedObject.name);

            this.curBuildingNum = int.Parse(gesture.pickedObject.name);

            MessageManager.GetSingleton().SendMsg("OnTap_" + this.curBuildingNum,new object[] { this.curBuildingNum });
        }
    }
}
