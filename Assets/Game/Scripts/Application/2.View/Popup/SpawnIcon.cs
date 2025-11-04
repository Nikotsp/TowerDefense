using UnityEngine;
using System.Collections;
using System;

public class SpawnIcon : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    TowerInfo m_Info;
    Vector3 m_Position;
    bool m_IsEnouh;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Load(GameModel gm, TowerInfo towerInfo, Vector3 position, bool isUpside)
    {
        //存储信息  
        m_Info = towerInfo;
        m_Position = position;
        //判断金币是否足够
        m_IsEnouh = gm.Gold >= towerInfo.BasePrice;
        m_IsEnouh = true;
        //加载图片
        string path = "Res/Roles/" + (m_IsEnouh ? m_Info.NormalIcon : m_Info.DisabledIcon);

        spriteRenderer.sprite = Resources.Load<Sprite>(path);
        //摆放位置
        Vector3 locPos = transform.localPosition;
        locPos.y = isUpside ? Mathf.Abs(locPos.y) : -Mathf.Abs(locPos.y);
        transform.localPosition = locPos;
    }
    void OnMouseDown()
    {
        //金币是否足够
        if (!m_IsEnouh)
            return;
        //创建什么塔
        int towerID = m_Info.ID;
        //创建的位置
        Vector3 position = m_Position;
        //事件参数
        object[] args = new object[] { towerID, position };
        //消息冒泡
        SendMessageUpwards("OnSpawnTower", args, SendMessageOptions.RequireReceiver);
    }
}