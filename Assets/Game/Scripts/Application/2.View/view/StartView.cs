using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class StartView : View
{
    public override string Name
    {
        get { return Consts.V_Start; }
    }
    public override void HandleEvent(string eventName, object data)
    {

    }
    public void OnStartButtonClick()
    {
        Game.Instance.LoadScene(2);
    }
}
