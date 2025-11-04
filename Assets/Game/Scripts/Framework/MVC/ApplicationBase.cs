using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationBase<T> : Singleton<T>
    where T : MonoBehaviour
{
    public void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }
    public void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }
}
