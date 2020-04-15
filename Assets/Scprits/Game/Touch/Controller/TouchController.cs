using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

namespace GameScene
{
    /// <summary>
    /// 触碰控制器
    /// </summary>
    public class TouchController : MonoBehaviour
    {

       public void OnTap(Gesture gesture)
        {
            Debug.Log("当前Tap建筑的序号是：" + gesture.pickedObject.name);

            /*
            int buildingNum = int.Parse(gesture.pickedObject.name);
            if (buildingNum >= 0 && buildingNum <= 8)
            {
                MessageManager.GetSingleton().SendMessage("PopupMainUI_ShowView");
            }
            else
            {
                return;
            }
            */
        }

    }
}
