using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override void Move(GameObject spot)
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

    }
}
