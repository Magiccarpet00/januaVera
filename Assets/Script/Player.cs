using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override void Move(GameObject spot) //[CODE REFACTOT] move fait trop de chose
    {
        base.Move(spot);

        //Enlever les nuages
        Vector3 v = spot.transform.parent.position;
        List<GameObject> tiles = GameManager.instance.GetTiles(((int)v.x) / 10 - 1, ((int)v.y / 10) - 1, ((int)v.x / 10) + 1, ((int)v.y / 10) + 1);
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponentInChildren<Tile>();
            t.CleanCloud();
        }

        if (GetCurrentSpot() != null && GetCurrentSpot().transform.childCount == 1)
        {
            GameManager.instance.UpdateUILandscape(GetCurrentLocationType());
        }
        else
        {
            GameManager.instance.UpdateUILandscape();
        }
    }



    //
    //      GET & SET
    //
    public override bool isPlayer()
    {
        return true;
    }
}
