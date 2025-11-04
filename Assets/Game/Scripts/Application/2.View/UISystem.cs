using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISystem : View
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
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
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
        Hide();
    }

    public void OnRestartClick()
    {
        GameModel gm = (GameModel)GetModel<GameModel>();
        SendEvent(Consts.E_StartLevel, new StartLevelArgs() { LevelIndex = gm.PlayLevelIndex });

    }

    public void OnSelectClick()
    {
        GameModel gm = (GameModel)GetModel<GameModel>();
        SendEvent(Consts.E_EnterScene, new SceneArgs() { SceneIndex = 2 });
    }
    #endregion

    #region 帮助方法
    #endregion
}
