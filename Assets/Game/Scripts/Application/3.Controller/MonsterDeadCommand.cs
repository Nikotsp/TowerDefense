using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadCommand : Controller
{
    public override void Execute(object data)
    {
        MonsterDeadArgs monsterDeadArgs = new MonsterDeadArgs();
        GameModel gameModel = (GameModel)GetModel<GameModel>();
        gameModel.Gold += monsterDeadArgs.Score;

    }


}
