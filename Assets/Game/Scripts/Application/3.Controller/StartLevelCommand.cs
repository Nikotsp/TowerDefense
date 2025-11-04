using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

class StartLevelCommand : Controller
{
    public override void Execute(object data)
    {
        StartLevelArgs e = data as StartLevelArgs;
        //第一步      
        GameModel gameModel = (GameModel)GetModel<GameModel>();
        gameModel.StartLevel(e.LevelIndex);

        //第二步
        RoundModel roundModel = (RoundModel)GetModel<RoundModel>();
        roundModel.LoadLevel(gameModel.PlayLevel);
        //进入游戏
        Game.Instance.LoadScene(3);
    }
}