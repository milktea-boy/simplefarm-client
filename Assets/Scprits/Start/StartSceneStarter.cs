using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 开始场景启动器
/// </summary>
public class StartSceneStarter : MonoBehaviour {

    private GameObject frameworkObj;

    private void Awake()
    {
        frameworkObj = GameObject.Find("Framework");
    }

    private void Start()
    {
        // 框架对象不销毁
        DontDestroyOnLoad(frameworkObj);
    }

    private void Update()
    {
        // 网络连接成功前不进入登录场景
        if (!Constant.Constants.socketConnected)
            return;

        SceneManager.LoadScene("Login");
    }

}
