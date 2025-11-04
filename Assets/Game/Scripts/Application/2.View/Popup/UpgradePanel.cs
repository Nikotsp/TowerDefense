using UnityEngine;
using System.Collections;

public class UpgradePanel : MonoBehaviour
{
    UpgradeIcon m_UpgradeIcon;
    SellIcon m_SellIcon;
    void Awake()
    {
        m_UpgradeIcon = GetComponentInChildren<UpgradeIcon>();
        m_SellIcon = GetComponentInChildren<SellIcon>();
    }
    public void Show(Tower tower)
    {
        transform.position = tower.transform.position;

        //显示
        m_UpgradeIcon.Load(tower);
        m_SellIcon.Load(tower);

        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}