using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionFight : Action
{
    public ActionFight(Character u) : base(GlobalConst.FIGHT_PRIORITY, u) {;}
    public override void PerfomAction()
    {
        GameManager.instance. StartFight(GetUser().GetCurrentSpot().GetComponent<Spot>().GetAllCharactersAliveOnMapInSpot(), GetUser());
    }
}
