using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerDownHandler
{
    //事件声明
    public event Action<Card> OnClickCard;
    public Image ImgCard;
    public Image ImgLock;

    //关卡属性
    Card m_Card = null;

    void Awake()
    {
        this.ImgCard = GetComponent<Image>();
        this.ImgLock = transform.GetChild(0).GetComponent<Image>();
    }

    public void DataBind(Card card)
    {
        m_Card = card;
        //加载关卡图片
        string cardFile = "file://" + Consts.CardDir + "\\" + m_Card.CardImage;
        StartCoroutine(Tools.LoadImage(cardFile, ImgCard));
        ImgLock.gameObject.SetActive(card.IsLocked);
    }
    //派发事件
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnClickCard != null)
        {
            OnClickCard.Invoke(m_Card);
        }
    }

    //取消订阅
    void Oestroy()
    {
        while (OnClickCard != null)
        {
            OnClickCard -= OnClickCard;
        }
    }
}
