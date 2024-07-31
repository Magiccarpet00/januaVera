using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestApple : Quest
{

    public int req_nbGoldenAppleTake;
    public ObjectData goldenApple;
    public ObjectData goldenPotion;

    public override void _Reward()
    {
        GameManager.instance.playerCharacter.AddObject(GameManager.instance.CreateObject(goldenPotion));
    }

    public override bool Test_4_CAN_FINISH()
    {
        if (GameManager.instance.playerCharacter.CountItem(goldenApple) >= req_nbGoldenAppleTake)
            return true;
        else
            return false;
    }
    
    public override void StartQuest()
    {
        base.StartQuest();
        Vector3 v = questGiver.GetCurrentSpot().transform.parent.position;

        for (int i = 0; i < 3; i++)
        {
            List<GameObject> tiles = GameManager.instance.GetTiles(((int)v.x) / 10 - 1, ((int)v.y / 10) - 1, ((int)v.x / 10) + 1, ((int)v.y / 10) + 1);
            int rng = Random.Range(0, tiles.Count);

            List<Spot> spots = tiles[rng].GetComponentInChildren<Tile>().GetSpots();
            int rng2 = Random.Range(0, spots.Count);

            spots[rng2].GetComponent<Spot>().AddObject(GameManager.instance.CreateObject(goldenApple));
        }
    }

    protected override string Dialog_1_NOT_REQUIREMENTS()
    {
        return "Hi";
    }

    protected override string Dialog_2_CAN_START()
    {
        return "I need 3 golden apples to finish my potion.";
    }

    protected override string Dialog_3_IN_PROGRES()
    {
        return "I need 3 golden apples to finish my potion.";
    }

    protected override string Dialog_4_CAN_FINISH()
    {
        return "Ohhh thank you for your help traveller.";
    }

    protected override string Dialog_5_FINISHED()
    {
        return "Mhhhhh My potion is delicious, I'm the king of soup!";
    }


}
