using UnityEngine;

public class ActionHide : Action
{
    public ActionHide(Character u) : base(GlobalConst.HIDE_PRIORITY,u) {;}

    public override void PerfomAction()
    {
        base.GetUser().Hide();
    }
}
