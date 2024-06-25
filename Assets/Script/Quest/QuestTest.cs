using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : Quest
{
    public int nbScorpionKill;
    public int req_nbScorpionKill;

    public override bool RequirementsFinish()
    {
        if (nbScorpionKill == req_nbScorpionKill)
            return true;
        else
            return false;
    }

    public override void Reward()
    {
        Debug.Log("Reward");
    }
}
