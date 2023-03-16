using UnityEngine;

public class Player : Character
{
    public override void Move(GameObject spot) //[CODE REFACTOT] move fait trop de chose
    {
        base.Move(spot);

        GameManager.instance.AddTurn(1);

        if (GetCurrentSpot().transform.childCount == 1)
        {
            GameManager.instance.UpdateUILandscape(GetCurrentLocationType());
        }
        else
        {
            GameManager.instance.UpdateUILandscape();
        }


        GameManager.instance.UpdateUIButtonGrid(GetCurrentButtonAction());

    }
}
