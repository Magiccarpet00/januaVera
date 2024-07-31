using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : Quest
{
    public CharacterData scorpionData;
    private Character scorpion;

    private int goldReward = 5;

    protected override string Dialog_1_NOT_REQUIREMENTS()
    {
        return "Welcome wanderer";
    }

    protected override string Dialog_2_CAN_START()
    {
        return "There's a scorpion nearby. Can you kill it?";
    }

    protected override string Dialog_3_IN_PROGRES()
    {
        return "You still haven't killed the scorpion!";
    }

    protected override string Dialog_4_CAN_FINISH()
    {
        return "Thanks for your help wanderer \n +"+goldReward.ToString() +" gold";
    }

    protected override string Dialog_5_FINISHED()
    {
        return "The hamlet is quieter thanks to you";
    }


    public override void StartQuest()
    {
        base.StartQuest();
        Vector3 v = questGiver.GetCurrentSpot().transform.parent.position;

        List<GameObject> tiles = GameManager.instance.GetTiles(((int)v.x) / 10 - 1, ((int)v.y / 10) - 1, ((int)v.x / 10) + 1, ((int)v.y / 10) + 1);
        int rng = Random.Range(0, tiles.Count);

        List<Spot> spots = tiles[rng].GetComponentInChildren<Tile>().GetSpots();
        int rng2 = Random.Range(0, spots.Count);

        GameObject go = GameManager.instance.CreateCharacter(scorpionData, spots[rng2].gameObject);
        scorpion = go.GetComponent<Character>();
    }

    public override bool Test_4_CAN_FINISH()
    {
        if (scorpion == null) return false;

        return scorpion.isDead;
    }

    public override void _Reward()
    {
        GameManager.instance.playerCharacter.AddGold(goldReward);
    }
}
