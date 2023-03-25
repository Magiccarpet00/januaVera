using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRest : Action
{
    public ActionRest(Character u) : base(GlobalConst.EMPTY_PRIORITY, u) {; }
    public override void PerfomAction()
    {
        base.GetUser().Rest();
    }
}
