using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnPanel : MonoBehaviour
{
    SpawnIcon[] m_SpawnIcon;
    void Awake()
    {
        m_SpawnIcon = GetComponentsInChildren<SpawnIcon>();
    }
    public void Show(GameModel gm, Vector3 position, bool isUpside)
    {
        //动态加载图标
        for (int i = 0; i < m_SpawnIcon.Length; i++)
        {
            TowerInfo info = Game.Instance.StaticData.GetTowerInfo(i);
            m_SpawnIcon[i].Load(gm, info, position, isUpside);
        }
        //设置位置
        transform.position = position;
        //显示
        transform.gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
