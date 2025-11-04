using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MVC
{
    public static Dictionary<string, Model> Models = new();
    public static Dictionary<string, View> Views = new();
    public static Dictionary<string, Type> CommandMap = new();

    //注册
    public static void RegisterModel(Model model)
    {
        Models[model.Name] = model;
    }
    public static void RegisterView(View view)
    {
        if (Views.ContainsKey(view.Name))
        {
            Debug.LogError("视图层重复注册，请检查！  view:" + view.Name);
        }

        Views[view.Name] = view;
    }
    public static void RegisterController(string eventName, Type controllerType)
    {
        CommandMap[eventName] = controllerType;
    }
    //取消注册
    public static void UnRegisterView(View view)
    {
        if (!Views.ContainsKey(view.Name))
        {
            return;
        }
        Views.Remove(view.Name);
    }

    //获取
    public static Model GetModel<T>()
        where T : Model
    {
        foreach (Model m in Models.Values)
        {
            if (m is T)
            {
                return m;
            }
        }
        return null;
    }
    public static View GetView<T>()
        where T : View
    {
        foreach (View v in Views.Values)
        {
            if (v is T)
            {
                return v;
            }
        }
        return null;
    }
    //事件消息发送
    public static void SendEvent(string eventName, object data = null)
    {
        //控制器
        if (CommandMap.ContainsKey(eventName))
        {   //获取控制器类型
            Type t = CommandMap[eventName];
            //动态创建
            Controller c = Activator.CreateInstance(t) as Controller;
            //执行
            c.Execute(data);
        }
        //视图
        foreach (var v in Views.Values)
        {
            if (v.AttentionList.Contains(eventName))
            {
                v.HandleEvent(eventName, data);
            }
        }
    }
}
