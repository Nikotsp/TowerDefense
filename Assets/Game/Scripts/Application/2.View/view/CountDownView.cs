using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CountDownView : View
{
    public Image Count;
    public Sprite[] Numbers;
    public override string Name
    {
        get
        {
            return Consts.V_CountDown;
        }
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Initialize()
    {
        base.Initialize();
        Count = this.transform.Find("Count").GetComponent<Image>();
    }

    #region  事件回调
    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AttentionList.Add(Consts.E_EnterScene);
    }
    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_EnterScene:
                SceneArgs e = (SceneArgs)data;
                if (e.SceneIndex == 3)
                {
                    StartCoroutine(DisplayCount());
                }
                break;
        }
    }

    IEnumerator DisplayCount()
    {
        int count = 3;
        while (count > 0)
        {
            Count.sprite = Numbers[count - 1];
            count--;

            yield return new WaitForSeconds(1f);
            if (count <= 0)
            {
                break;
            }
        }
        SetActive(false);
        SendEvent(Consts.E_CountDownComplete);
    }
    #endregion
}
