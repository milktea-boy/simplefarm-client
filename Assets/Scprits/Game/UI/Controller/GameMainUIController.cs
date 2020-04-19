using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameScene
{
    /// <summary>
    /// 游戏主菜单控制器
    /// </summary>
    public class GameMainUIController : BaseController<GameMainUIModel,GameMainUIView>
    {
        void Awake()
        {
            base.Awake();

            MessageManager.GetSingleton().RegisterMessageListener("Initialization_MainUIInfo", SetData);
        }

        private void OnDestroy()
        {
            MessageManager.GetSingleton().UnRegisterMessageListener("Initialization_MainUIInfo", SetData);
        }

        private void SetData(object data)
        {
            object[] args;
            args = (object[])data;
            View.textGrade.text = args[0].ToString();
            View.textGoldCoins.text = args[1].ToString();
            View.textNickName.text = args[2].ToString();

            //升级经验盘数值的换算0-1之间(是否该自动升级后续找后端商讨)
            int exp = int.Parse(args[3].ToString());
            int needExp = int.Parse(args[4].ToString());
            if (exp <= needExp)
            {
                View.circleImageExpImage.fillPercent = exp / needExp;
            }
        }
    }
}
