using UnityEngine;
using System.Collections;

public class UpgradeIcon : MonoBehaviour
{
    SpriteRenderer m_Render;
    Tower m_Tower;
    void Awake()
    {
        m_Render = GetComponent<SpriteRenderer>();
    }
    public void Load(Tower tower)
    {
        //保存数据
        this.m_Tower = tower;
        //显示图片
        TowerInfo info = Game.Instance.StaticData.GetTowerInfo(tower.ID);
        string path = "Res/Roles/" + (m_Tower.IsTopLevel ? info.DisabledIcon : info.NormalIcon);
        m_Render.sprite = Resources.Load<Sprite>(path);
    }
    void OnMouseDown()
    {
        Tower tower = m_Tower;
        Debug.Log("upgrade");
        SendMessageUpwards("OnUpgradeTower", tower);
    }
}