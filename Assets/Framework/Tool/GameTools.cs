/**
 * 常用游戏工具类
 * 使用方法：
 *     1、编辑界面FrameWork->Create FrameWork Object建立框架对象
 *     2、GameTools.GetSingleton()获取单例
 *     3、调用你要使用的方法
 */

using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.UI;

public class GameTools : MonoBehaviour {

    private static GameTools mng;

    public static GameTools GetSingleton()
    {
        return mng;
    }

    private void Awake()
    {
        if (mng == null)
            mng = this;
    }

    /// <summary>
    /// 截图
    /// </summary>
    /// <param name="x">起始x坐标</param>
    /// <param name="y">起始y坐标</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <returns></returns>
    public Texture2D PrintScreenTexture(int x, int y, int width, int height)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(x, y, width, height), 0, 0);
        return tex;
    }

    /// <summary>
    /// 截图
    /// </summary>
    /// <param name="x">起始x坐标</param>
    /// <param name="y">起始y坐标</param>
    /// <param name="width">宽度</param>
    /// <param name="height">高度</param>
    /// <returns></returns>
    public Sprite PrintScreenSprite(int x, int y, int width, int height)
    {
        Texture2D tex = PrintScreenTexture(x, y, width, height);
        Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return s;
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="path">保存文件的完整地址</param>
    /// <param name="file">文件</param>
    /// <returns></returns>
    public bool SaveFileToLocal(string path, byte[] file)
    {
        try
        {
            string filePath = path.Substring(0, path.LastIndexOf('/') + 1);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            File.WriteAllBytes(path, file);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 加载文件
    /// </summary>
    /// <param name="path">文件的完整路径</param>
    /// <returns></returns>
    public byte[] LoadFileFromLocal(string path)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int) fs.Length);
            fs.Close();
            fs.Dispose();
            return bytes;
    }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 加载网络图片
    /// </summary>
    /// <param name="url">图片地址</param>
    /// <param name="callback">加载完成回调</param>
    public void GetTextureFromNet(string url, System.Action<Texture2D> callback)
    {
        if(callback == null)
            return;
        StartCoroutine(LoadTexture(url, callback));
    }

    private IEnumerator LoadTexture(string url, System.Action<Texture2D> callback)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("加载图片失败：" + url);
        }
        else
        {
            callback(Instantiate(www.texture));
        }
        www.Dispose();
    }

    /// <summary>
    /// 加载网络图片
    /// </summary>
    /// <param name="url">图片地址</param>
    /// <param name="callback">加载完成回调</param>
    public void GetTextureFromNet(string url, System.Action<Sprite> callback)
    {
        if (callback == null)
            return;
        StartCoroutine(LoadTexture(url, callback));
    }

    private IEnumerator LoadTexture(string url, System.Action<Sprite> callback)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("加载图片失败：" + url);
        }
        else
        {
            Sprite s = Sprite.Create(Instantiate(www.texture), new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            callback(s);
        }
        www.Dispose();
    }

    /// <summary>
    /// 加载JSON文件
    /// </summary>
    /// <param name="url">文件地址</param>
    /// <param name="callback">加载完成回调</param>
    /// <param name="fromLocal">是否从本地加载</param>
    public void LoadJson(string url, Action<Hashtable> callback, bool fromLocal = false)
    {
        if(string.IsNullOrEmpty(url)|| callback == null)
            return;
        if (fromLocal && !(url.Contains("file://") || url.Contains("file:///")))
            url = "file:///" + url;
        StartCoroutine(LoadJsonCoroutine(url, callback));
    }

    private IEnumerator LoadJsonCoroutine(string url, Action<Hashtable> callback)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
            Debug.Log("加载JSON失败：" + www.error);
        else
        {
            try
            {
                Hashtable data = TinyJSON.jsonDecode(www.text) as Hashtable;
                callback(data);
            }
            catch
            {
                Debug.Log("JSON格式错误");
            }
        }
        www.Dispose();
    }

    /// <summary>
    /// 加载JSON文件
    /// </summary>
    /// <param name="url">文件地址</param>
    /// <param name="callback">加载完成回调</param>
    /// <param name="fromLocal">是否从本地加载</param>
    public void LoadJson(string url, Action<string> callback, bool fromLocal = false)
    {
        if (string.IsNullOrEmpty(url) || callback == null)
            return;
        if (fromLocal && !(url.Contains("file://") || url.Contains("file:///")))
            url = "file:///" + url;
        StartCoroutine(LoadJsonCoroutine(url, callback));
    }

    private IEnumerator LoadJsonCoroutine(string url, Action<string> callback)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
            Debug.Log("加载JSON失败：" + www.error);
        else
        {
            try
            {
                string s = www.text;
                callback(s);
            }
            catch
            {
                Debug.Log("JSON格式错误");
            }
        }
        www.Dispose();
    }

    /// <summary>
    /// 保存json文件到本地
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="json">json数据</param>
    /// <returns></returns>
    public bool SaveJsonToLocal(string path, string json)
    {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(json);
        bool result = SaveFileToLocal(path, bytes);
        return result;
    }

    /// <summary>
    /// 保存json文件到本地
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="jsonData">json数据</param>
    /// <returns></returns>
    public bool SaveJsonToLocal(string path, Hashtable jsonData)
    {
        string s = TinyJSON.jsonEncode(jsonData);
        bool result = SaveJsonToLocal(path, s);
        return result;
    }
}
