using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUnHide : Action
{
    public ActionUnHide(Character u) : base(GlobalConst.HIDE_PRIORITY, u) {; }
    public override void PerfomAction()
    {
        base.GetUser().UnHide();
    }
}
