using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PlaySpeed
{
    One = 1,
    Two = 2,
}
public class MenuView : View
{
    public override string Name
    {
        get
        {
            return Consts.V_Board;
        }
    }
    private int m_score;
    private int curRound;
    private int totalRound;
    private PlaySpeed playSpeed;
    private bool isPlaying = false;

    private Text txtScore;
    private Text txtCurRound;
    private Text txtTotalRound;
    private GameObject objBtnOne;
    private GameObject objBtnTwo;
    private GameObject objBtnResume;
    private GameObject objBtnPause;
    private GameObject objBtnSystem;
    private GameObject PauseInfo;
    #region 属性
    public int Score
    {
        get { return m_score; }
        set
        {
            m_score = Mathf.Clamp(value, 0, int.MaxValue);
            this.txtScore.text = this.m_score.ToString();
        }
    }
    public int CurRound
    {
        get { return this.curRound; }
        set
        {
            curRound = Mathf.Clamp(value, 0, int.MaxValue);
            this.txtCurRound.text = this.curRound.ToString();
        }
    }
    public int TotalRound
    {
        get { return this.totalRound; }
        set
        {
            totalRound = Mathf.Clamp(value, 0, int.MaxValue);
            this.txtTotalRound.text = this.totalRound.ToString("D2");
        }
    }
    public PlaySpeed PlaySpeed
    {
        get { return playSpeed; }
        set
        {
            playSpeed = value;
            objBtnOne.gameObject.SetActive(playSpeed == PlaySpeed.One);
            objBtnTwo.gameObject.SetActive(playSpeed == PlaySpeed.Two);
        }
    }
    public bool IsPlaying
    {
        get { return isPlaying; }
        set
        {
            this.isPlaying = value;
            this.objBtnResume.gameObject.SetActive(!this.isPlaying);
            this.objBtnPause.gameObject.SetActive(this.isPlaying);
            PauseInfo.SetActive(!this.isPlaying);
        }
    }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        RoundModel rm = (RoundModel)GetModel<RoundModel>();
        CurRound = rm.RoundIndex + 1;
        TotalRound = rm.RoundTotal;
        rm.OnChangeRound += UpdateRoundInfo;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        RoundModel rm = (RoundModel)GetModel<RoundModel>();
        rm.OnChangeRound -= UpdateRoundInfo;
    }

    protected override void Initialize()
    {
        base.Initialize();
        txtScore = this.transform.Find("Score").GetComponent<Text>();
        Transform roundInfo = this.transform.Find("RoundInfo");
        txtCurRound = roundInfo.Find("txtCurrent").GetComponent<Text>();
        txtTotalRound = roundInfo.Find("txtTotal").GetComponent<Text>();
        objBtnOne = this.transform.Find("BtnSpeed1").gameObject;
        objBtnOne.GetComponent<Button>().onClick.AddListener(OnOneClick);

        objBtnTwo = this.transform.Find("BtnSpeed2").gameObject;
        objBtnTwo.GetComponent<Button>().onClick.AddListener(OnTwoClick);

        objBtnPause = this.transform.Find("BtnPause").gameObject;
        objBtnPause.GetComponent<Button>().onClick.AddListener(OnPauseClick);

        objBtnResume = this.transform.Find("BtnResume").gameObject;
        objBtnResume.GetComponent<Button>().onClick.AddListener(OnResumeClick);

        objBtnSystem = this.transform.Find("BtnSystem").gameObject;
        objBtnSystem.GetComponent<Button>().onClick.AddListener(OnSystemClick);
        PauseInfo = this.transform.Find("PauseInfo").gameObject;

        objBtnTwo.SetActive(false);
        PauseInfo.SetActive(false);
        GameModel gameModel = (GameModel)GetModel<GameModel>();
        Score = gameModel.Gold;
    }
    void UpdateRoundInfo(object sender, ChangeRoundArgs e)
    {
        CurRound = e.CurRound;
    }
    #region 事件
    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AttentionList.Add(Consts.E_GoldChange);
    }
    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_GoldChange:
                GoledChangeArgs e = data as GoledChangeArgs;
                Score = e.Value;
                break;
        }
    }
    private void OnOneClick()
    {
        this.PlaySpeed = PlaySpeed.Two;
        Time.timeScale = 2;
    }
    private void OnTwoClick()
    {
        this.PlaySpeed = PlaySpeed.One;
        Time.timeScale = 1;
    }
    private void OnPauseClick()
    {
        this.IsPlaying = false;
        Time.timeScale = 0;
    }
    private void OnResumeClick()
    {
        this.IsPlaying = true;
        Time.timeScale = 1;
    }
    private void OnSystemClick()
    {
        SystemView systemView = (SystemView)GetView<SystemView>();
        systemView.SetActive(true);
        Time.timeScale = 0;
    }
    #endregion
}
