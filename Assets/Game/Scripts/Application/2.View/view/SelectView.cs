using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectView : View
{
    private List<Level> levels = new();
    private List<Card> m_cards = new();
    private int m_selectIndex = -1;
    GameModel gameModel = null;
    public Button StartBtn;
    public override string Name
    {
        get { return Consts.V_Select; }
    }

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
                SceneArgs scene = data as SceneArgs;
                if (scene.SceneIndex == 2)
                {
                    //获取模型数据
                    gameModel = (GameModel)GetModel<GameModel>();
                    //初始化列表
                    LoadCards();
                }
                break;
        }
    }
    //返回开始场景
    public void GoBack()
    {
        Game.Instance.LoadScene(1);
    }
    public void ChooseLevel()
    {
        StartLevelArgs startCommand = new StartLevelArgs();
        startCommand.LevelIndex = m_selectIndex;
        SendEvent(Consts.E_StartLevel, startCommand);
    }

    private void LoadCards()
    {
        List<Level> levelList = gameModel.AllLevels;
        List<Card> cards = new List<Card>();
        for (int i = 0; i < levelList.Count; i++)
        {
            Card card = new Card()
            {
                LevelID = i,
                CardImage = levelList[i].CardImage,
                IsLocked = i > gameModel.GameProgress + 1
            };
            cards.Add(card);
        }
        //保存当前关卡信息
        m_cards = cards;
        //监听卡片点击事件
        CardUI[] cardUIs = this.transform.Find("UICards").GetComponentsInChildren<CardUI>();
        if (cardUIs != null)
        {
            foreach (CardUI cardUI in cardUIs)
            {
                //注册事件
                cardUI.OnClickCard += (card) =>
                {
                    SelectCard(card.LevelID);
                };
            }
        }
        //默认选中第一个卡片
        SelectCard(0);
    }

    private void SelectCard(int SelectIndex)
    {
        if (m_selectIndex == SelectIndex) return;
        m_selectIndex = SelectIndex;

        //计算索引号
        int left = m_selectIndex - 1;
        int current = m_selectIndex;
        int right = m_selectIndex + 1;

        //绑定数据
        Transform container = this.transform.Find("UICards");

        //左边
        if (left < 0)
        {
            container.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            container.GetChild(0).gameObject.SetActive(true);
            container.GetChild(0).GetComponent<CardUI>().DataBind(m_cards[left]);
        }

        //当前
        container.GetChild(1).GetComponent<CardUI>().DataBind(m_cards[current]);

        //当前开始按钮
        StartBtn.gameObject.SetActive(!m_cards[current].IsLocked);

        //右边
        if (right >= m_cards.Count)
        {
            container.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            container.GetChild(2).gameObject.SetActive(true);
            container.GetChild(2).GetComponent<CardUI>().DataBind(m_cards[right]);
        }

    }
}
