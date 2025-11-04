using UnityEngine;
using System.Collections;

public class SellIcon : MonoBehaviour
{
    SpriteRenderer sprite;
    Tower m_Tower;

    public void Load(Tower tower)
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = Resources.Load<Sprite>("Res/Scene/Items02-hd.pvr/sell_80");
        m_Tower = tower;
    }

    void OnMouseDown()
    {
        Tower tower = m_Tower;
        Debug.Log("sell");
        SendMessageUpwards("OnSellTower", tower, SendMessageOptions.DontRequireReceiver);
    }
}