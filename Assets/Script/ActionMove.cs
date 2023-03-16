using UnityEngine;

public class ActionMove : Action
{
    private GameObject destinationSpot;
    public ActionMove(int p, Character u, GameObject spot) : base(p,u)
    {
        destinationSpot = spot;
    }

    public override void PerfomAction()
    {
        base.GetUser().Move(destinationSpot);
    }
    
}
