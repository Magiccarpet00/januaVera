using UnityEngine;

public class ActionMove : Action
{
    private GameObject destinationSpot;
    private bool isHideMove;
    public ActionMove(Character u, GameObject spot, bool _hideMove = false) : base(GlobalConst.MOVE_PRIORITY, u)
    {
        destinationSpot = spot;
        isHideMove = _hideMove;
        
    }

    public override void PerfomAction()
    {
        base.GetUser().Move(destinationSpot);
    }
    
}
