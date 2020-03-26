using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MVCController基类
/// </summary>
/// <typeparam name="M">Model类型</typeparam>
/// <typeparam name="V">View类型</typeparam>
public abstract class BaseController<M, V> : MonoBehaviour {

    private M model;
    private V view;

    /// <summary>
    /// Model对象
    /// </summary>
    public M Model {
        get
        {
            return model;
        }
    }

    /// <summary>
    /// View对象
    /// </summary>
    public V View
    {
        get
        {
            return view;
        }
    }

    public void Awake()
    {
        model = gameObject.GetComponent<M>();
        view = gameObject.GetComponent<V>();
    }

}
