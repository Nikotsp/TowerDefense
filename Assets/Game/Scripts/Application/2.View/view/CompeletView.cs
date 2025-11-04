using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompeletView : View
{
    public Button BtnSelect;
    public Button BtnClear;
    public override string Name => Consts.V_Complete;
    protected override void RegisterEvents()
    {
        base.RegisterEvents();
    }
    protected override void Initialize()
    {
        base.Initialize();
        BtnClear = this.transform.Find("BtnSelect").GetComponent<Button>();
        BtnClear.onClick.AddListener(OnClickSelect);
        BtnSelect = this.transform.Find("BtnClear").GetComponent<Button>();
        BtnSelect.onClick.AddListener(OnClickClear);
    }

    public override void HandleEvent(string eventName, object data)
    {
        throw new System.NotImplementedException();
    }
    public void OnClickSelect()
    {
        Game.Instance.LoadScene(1);
    }
    public void OnClickClear()
    {
        GameModel game = (GameModel)GetModel<GameModel>();
        game.ClearProgress();
    }

}
