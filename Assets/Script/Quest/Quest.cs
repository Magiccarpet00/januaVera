using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    [SerializeField]
    private QuestState questState;
    public void StartQuest()
    {
        questState = QuestState.IN_PROGRES;
    }

    public void UpdateState()
    {
        switch (questState)
        {
            case QuestState.NOT_REQUIREMENTS:
                break;
            case QuestState.CAN_START:
                break;
            case QuestState.IN_PROGRES:
                if(RequirementsFinish())
                {
                    questState = QuestState.CAN_FINISH;
                    Reward();
                }
                break;
            case QuestState.CAN_FINISH:
                break;
            case QuestState.FINISHED:
                break;
            default:
                break;
        }
    }

    public abstract bool RequirementsFinish();
    public abstract void Reward();
}

public enum QuestState{
    NOT_REQUIREMENTS,
    CAN_START,
    IN_PROGRES,
    CAN_FINISH,
    FINISHED    
}
