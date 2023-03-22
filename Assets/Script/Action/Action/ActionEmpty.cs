using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEmpty : Action
{
    public ActionEmpty(Character u) : base(GlobalConst.EMPTY_PRIORITY, u) {;}
    public override void PerfomAction() {;}
}
