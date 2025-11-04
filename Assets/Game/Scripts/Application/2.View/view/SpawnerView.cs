using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerView : View
{
    public Map m_Map;
    public Luobo m_Luobo = null;
    public override string Name => Consts.E_SpawnMonster;

    public void SpawnLuobo(Vector3 endPos)
    {
        GameObject go = Game.Instance.ObjectPool.Spawn("Luobo");
        m_Luobo = go.GetComponent<Luobo>();
        m_Luobo.Dead += luobo_Dead;
        m_Luobo.transform.position = endPos;
    }
    private void luobo_Dead(Role role)
    {
        GameModel gm = (GameModel)GetModel<GameModel>();
        Game.Instance.ObjectPool.Unspawn(role.gameObject);
        SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelID = gm.PlayLevelIndex, IsSuccess = false });
    }
    #region  怪物逻辑
    void SpawnMonster(int mosterID)
    {
        string PrefabName = "Monster" + mosterID;
        GameObject go = Game.Instance.ObjectPool.Spawn(PrefabName);
        Monster monster = go.GetComponent<Monster>();
        monster.Reached += monster_Reached;
        monster.Dead += monster_Dead;
        monster.HpChanged += monster_HpChanged;
        monster.Load(m_Map.Path);
    }
    void SpawnTower(int towerID, Vector3 position)
    {
        Tile tile = m_Map.GetTile(position);
        TowerInfo towerInfo = Game.Instance.StaticData.GetTowerInfo(towerID);
        GameObject go = Game.Instance.ObjectPool.Spawn(towerInfo.PrefabName);
        Tower tower = go.GetComponent<Tower>();
        tile.Data = tower;
        tower.transform.position = position;
        tower.Load(towerID, tile, m_Map.MapRect);
    }
    private void monster_HpChanged(int arg1, int arg2)
    {

    }

    private void monster_Dead(Role role)
    {
        GameModel gm = (GameModel)GetModel<GameModel>();
        RoundModel round = (RoundModel)GetModel<RoundModel>();
        if (!m_Luobo.IsDead     //萝卜没有死 
        && round.AllRoundsCompelet  //所有回合已经出完
        )
        {
            GameObject[] allMonster = GameObject.FindGameObjectsWithTag("Monster");
            if (allMonster.Length == 0)     //场上没有其他怪物
                SendEvent(Consts.E_EndLevel, new EndLevelArgs() { LevelID = gm.PlayLevelIndex, IsSuccess = true });
        }
        Monster m = role as Monster;
        gm.Gold += m.Price;
        Game.Instance.ObjectPool.Unspawn(role.gameObject);
    }

    private void monster_Reached(Monster monster)
    {
        //萝卜掉血
        m_Luobo.Damage(1);
        monster.Hp = 0;
    }
    #endregion
    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AttentionList.Add(Consts.E_EnterScene);
        AttentionList.Add(Consts.E_SpawnMonster);
        AttentionList.Add(Consts.E_SpawnTower);
    }
    public override void HandleEvent(string eventName, object data)
    {
        switch (eventName)
        {
            case Consts.E_SpawnMonster:
                SpawnMonsterArgs spawnMonsterArgs = data as SpawnMonsterArgs;
                SpawnMonster(spawnMonsterArgs.MonsterID);
                break;
            case Consts.E_EnterScene:
                SceneArgs sceneArgs = data as SceneArgs;
                if (sceneArgs.SceneIndex == 3)
                {
                    GameModel game = (GameModel)GetModel<GameModel>();
                    //加载地图
                    m_Map = gameObject.GetComponent<Map>();
                    m_Map.LoadLevel(game.PlayLevel);
                    //加载萝卜
                    Vector3[] path = m_Map.Path;
                    Vector3 endPos = path[path.Length - 1];
                    m_Map.OnTileClick += MapOnTileClick;
                    SpawnLuobo(endPos);
                }
                break;
            case Consts.E_SpawnTower:
                SpawnTowerArgs e = data as SpawnTowerArgs;
                SpawnTower(e.TowerID, e.Position);
                break;
        }
    }

    private void MapOnTileClick(object sender, TileClickEventArgs e)
    {
        GameModel gm = (GameModel)GetModel<GameModel>();
        if (gm.IsPlaying && e.Tile.CanHold)
        {
            if (e.Tile.Data == null)
            {
                ShowCreateArgs e1 = new ShowCreateArgs();
                Vector3 position = m_Map.GetPosition(e.Tile);
                e1.Position = position;
                e1.UpSide = e.Tile.Y < (Map.RowCount / 2);
                SendEvent(Consts.E_ShowCreate, e1);
            }
            else
            {
                ShowUpgradeArgs e2 = new ShowUpgradeArgs();
                e2.Tower = e.Tile.Data as Tower;
                SendEvent(Consts.E_ShowUpgrade, e2);
            }
        }
    }
}
