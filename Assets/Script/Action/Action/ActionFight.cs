using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionFight : Action
{
    public ActionFight(Character u) : base(GlobalConst.FIGHT_PRIORITY, u) {;}
    public override void PerfomAction()
    {
        //BIG TODO
        Debug.Log("start battle");
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}
