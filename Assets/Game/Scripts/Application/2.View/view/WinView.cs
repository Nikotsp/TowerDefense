using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinView : View
{
    public Text txtCurrent;
    public Text txtTotal;
    public Button btnRestart;
    public Button btnContinue;
    public override string Name
    {
        get
        {
            return Consts.V_Win;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        SetActive(false);
        UpdateRoundInfo(0, 0);
    }
    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        RoundModel rm = (RoundModel)GetModel<RoundModel>();
        UpdateRoundInfo(rm.RoundIndex + 1, rm.RoundTotal);
    }
    protected override void Initialize()
    {
        base.Initialize();
        txtCurrent = transform.Find("txtCurrent").GetComponent<Text>();
        txtTotal = transform.Find("txtTotal").GetComponent<Text>();
        btnRestart = transform.Find("BtnRestart").GetComponent<Button>();
        btnRestart.onClick.AddListener(OnRestarClick);
        btnContinue = transform.Find("BtnContinue").GetComponent<Button>();
        btnContinue.onClick.AddListener(OnContinueClick);
    }
    public void UpdateRoundInfo(int currentRound, int totalRound)
    {
        txtCurrent.text = currentRound.ToString("D2");
        txtTotal.text = totalRound.ToString();
    }
    public override void HandleEvent(string eventName, object data)
    {

    }
    public void OnRestarClick()
    {
        GameModel gm = (GameModel)GetModel<GameModel>();
        SendEvent(Consts.E_StartLevel, new StartLevelArgs() { LevelIndex = gm.PlayLevelIndex });
    }
    public void OnContinueClick()
    {
        GameModel gm = (GameModel)GetModel<GameModel>();
        if (gm.PlayLevelIndex >= gm.LevelCount - 1)
        {
            Game.Instance.LoadScene(4);
        }
        else
        {
            SendEvent(Consts.E_StartLevel, new StartLevelArgs() { LevelIndex = gm.PlayLevelIndex + 1 });
        }
    }

}
