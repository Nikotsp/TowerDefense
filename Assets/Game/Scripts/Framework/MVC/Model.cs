using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Model
{
    public abstract string Name { get; }
    //通知视图
    public void SendEvent(string eventName, object data)
    {
        MVC.SendEvent(eventName, data);
    }
    public virtual void Initialize() { }
}
