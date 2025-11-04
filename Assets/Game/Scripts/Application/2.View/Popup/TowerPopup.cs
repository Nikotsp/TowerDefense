using UnityEngine;
using System.Collections;

public class TowerPopup : View
{
    SpawnPanel m_SpawnPanel;
    UpgradePanel m_UpgradePanel;
    public override string Name => Consts.V_TowerPopup;
    protected override void Initialize()
    {
        base.Initialize();
        m_SpawnPanel = GetComponentInChildren<SpawnPanel>();
        m_UpgradePanel = GetComponentInChildren<UpgradePanel>();
    }
    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AttentionList.Add(Consts.E_ShowCreate);
        AttentionList.Add(Consts.E_ShowUpgrade);
        AttentionList.Add(Consts.E_HidePopup);
    }
    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_ShowCreate:
                ShowCreateArgs e = data as ShowCreateArgs;
                ShowSpawnPanel(e.Position, e.UpSide);
                break;
            case Consts.E_ShowUpgrade:
                ShowUpgradeArgs ee = data as ShowUpgradeArgs;
                ShowUpgradePanel(ee.Tower);
                break;
            case Consts.E_HidePopup:
                HideAllPopups();
                break;
            default:
                break;
        }
    }
    void ShowSpawnPanel(Vector3 position, bool isUpside)
    {
        HideAllPopups();
        GameModel gm = (GameModel)GetModel<GameModel>();
        m_SpawnPanel.Show(gm, position, isUpside);
    }
    void ShowUpgradePanel(Tower tower)
    {
        HideAllPopups();
        m_UpgradePanel.Show(tower);
    }
    void HideAllPopups()
    {
        m_SpawnPanel.Hide();
        m_UpgradePanel.Hide();
    }

    void OnSpawnTower(object[] args)
    {
        int towerID = (int)args[0];
        Vector3 position = (Vector3)args[1];
        SpawnTowerArgs e = new SpawnTowerArgs() { TowerID = towerID, Position = position };
        SendEvent(Consts.E_SpawnTower, e);
    }
    void OnUpgradeTower(Tower tower)
    {
        UpgradeTowerArgs e = new UpgradeTowerArgs() { tower = tower };
        SendEvent(Consts.E_UpgradeTower, e);
    }
    void OnSellTower(Tower tower)
    {
        SendEvent(Consts.E_SellTower, new SellTowerArgs() { tower = tower });
    }

}
