using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EndLevelCommand : Controller
{
    public override void Execute(object data)
    {
        EndLevelArgs e = data as EndLevelArgs;
        //保存游戏状态
        GameModel gm = (GameModel)GetModel<GameModel>();
        gm.EndLevel(e.IsSuccess);
        //弹出UI
        if (e.IsSuccess)
        {
            GetView<WinView>().SetActive(true);
        }
        else
        {
            GetView<LostView>().SetActive(true);
        }
    }
}