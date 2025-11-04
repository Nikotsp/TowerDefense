using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SellTowerCommand : Controller
{
    public override void Execute(object data)
    {
        SellTowerArgs sellTowerArgs = data as SellTowerArgs;
        Tower tower = sellTowerArgs.tower;
        GameModel gm = (GameModel)GetModel<GameModel>();
        gm.Gold += (int)(tower.BasePrice * tower.Level * 0.8);
        tower.m_Tile.Data = null;
        tower.m_Tile.CanHold = true;
        Game.Instance.ObjectPool.Unspawn(tower.gameObject);
    }
}