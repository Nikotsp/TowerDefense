using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller
{
    public abstract void Execute(object data);
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
    protected void RegisterModel(Model model)
    {
        MVC.RegisterModel(model);
    }
    protected void RegisterView(View view)
    {
        MVC.RegisterView(view);
    }
    protected void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }
}
