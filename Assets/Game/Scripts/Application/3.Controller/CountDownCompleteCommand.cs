using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

class CountDownCompleteCommand : Controller
{
    public override void Execute(object data)
    {
        //开始游戏
        GameModel gModel = (GameModel)GetModel<GameModel>();
        gModel.IsPlaying = true;

        //开始出怪
        RoundModel rModel = (RoundModel)GetModel<RoundModel>();
        rModel.StartRound();
    }
}