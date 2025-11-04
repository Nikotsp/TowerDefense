using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LostView : View
{
    public Text txtCurrent;
    public Text txtTotal;
    public Button btnRestart;
    public override string Name => Consts.V_Lost;
    protected override void Awake()
    {
        base.Awake();
        SetActive(false);
    }
    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        RoundModel rm = (RoundModel)GetModel<RoundModel>();
        UpdateRoundInfo(rm.RoundIndex + 1, rm.RoundTotal);
        if (isActive)
        {
            rm.StopRound();
        }
    }
    protected override void Initialize()
    {
        base.Initialize();
        txtCurrent = transform.Find("txtCurrent").GetComponent<Text>();
        txtTotal = transform.Find("txtTotal").GetComponent<Text>();
        btnRestart = transform.Find("BtnRestart").GetComponent<Button>();
        btnRestart.onClick.AddListener(OnRestartClick);
    }
    public void UpdateRoundInfo(int currentRound, int totalRound)
    {
        txtCurrent.text = currentRound.ToString("D2");
        txtTotal.text = totalRound.ToString();
    }
    public override void HandleEvent(string eventName, object data)
    {
    }
    public void OnRestartClick()
    {
        GameModel gm = (GameModel)GetModel<GameModel>();
        SendEvent(Consts.E_StartLevel, new StartLevelArgs() { LevelIndex = gm.PlayLevelIndex });
    }
}
