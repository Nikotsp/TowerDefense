using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemView : View
{
    #region 常量
    #endregion

    #region 事件
    public Button btnResume;
    public Button btnRestart;
    public Button btnSelect;
    #endregion

    #region 字段
    #endregion

    #region 属性
    public override string Name
    {
        get { return Consts.V_Sytem; }
    }
    #endregion

    #region 方法
    protected override void Initialize()
    {
        base.Initialize();
        btnResume = this.transform.Find("BtnResume").GetComponent<Button>();
        btnResume.onClick.AddListener(OnResumeClick);

        btnRestart = this.transform.Find("BtnRestart").GetComponent<Button>();
        btnRestart.onClick.AddListener(OnRestartClick);

        btnSelect = this.transform.Find("BtnSelect").GetComponent<Button>();
        btnSelect.onClick.AddListener(OnSelectClick);

    }
    protected override void Awake()
    {
        base.Awake();
        SetActive(false);
    }
    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        this.gameObject.SetActive(isActive);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    public override void HandleEvent(string eventName, object data)
    {

    }

    public void OnResumeClick()
    {
        Time.timeScale = 1;
        SetActive(false);
    }

    public void OnRestartClick()
    {
        Time.timeScale = 1;
        GameModel gm = (GameModel)GetModel<GameModel>();
        SendEvent(Consts.E_StartLevel, new StartLevelArgs() { LevelIndex = gm.PlayLevelIndex });

    }

    public void OnSelectClick()
    {
        SendEvent(Consts.E_EnterScene, new SceneArgs() { SceneIndex = 2 });
    }
    #endregion

    #region 帮助方法
    #endregion
}
