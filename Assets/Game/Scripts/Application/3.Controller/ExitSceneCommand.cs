using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ExitSceneCommand : Controller
{
    public override void Execute(object data)
    {
        //离开场景全部回收
        Game.Instance.ObjectPool.UnspawnAll();
    }
}