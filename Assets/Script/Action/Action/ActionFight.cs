using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFight : Action
{
    public ActionFight(Character u) : base(GlobalConst.FIGHT_PRIORITY, u) {;}
    public override void PerfomAction()
    {
        //BIG TODO
        Debug.Log("start battle");
    }
}
