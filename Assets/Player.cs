using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override void Move(GameObject spot)
    {
        base.Move(spot);
        GameManager.instance.turn++;
    }
}
