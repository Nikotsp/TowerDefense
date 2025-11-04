using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public abstract string Name { get; }

    private List<string> attentionList = new List<string>();
    public List<string> AttentionList { get { return this.attentionList; } }
    public abstract void HandleEvent(string eventName, object data);
    protected Model GetModel<T>()
        where T : Model
    {
        return MVC.GetModel<T>();
    }
    protected View GetView<T>()
        where T : View
    {
        return MVC.GetView<T>();
    }
    protected void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }


    //注册关心事件
    protected virtual void RegisterEvents()
    {

    }

    protected virtual void Awake()
    {
        Initialize();
        RegisterEvents();
    }
    protected virtual void Initialize()
    {

    }
    public virtual void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }
    // protected virtual void OnEnable()S
    // {
    //     MVC.RegisterView(this);
    // }
    protected virtual void OnDisable()
    {
    }

    protected virtual void OnDestroy()
    {
        MVC.UnRegisterView(this);
    }
}
