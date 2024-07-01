using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    [SerializeField]
    private QuestState questState;
    public QuestState QuestState { get => questState;}

    public Character questGiver;

    public int nbTurn, nbTurnMax;


    public virtual void StartQuest() // CAN_START -> IN_PROGRES
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
                
                if(nbTurn == nbTurnMax)
                {
                    questState = QuestState.FAILED;
                }
                if(Test_4_CAN_FINISH())
                {
                    questState = QuestState.CAN_FINISH;
                    Reward();
                }
                nbTurn++;
                break;
            case QuestState.CAN_FINISH:
                break;
            case QuestState.FINISHED:
                break;
            default:
                break;
        }
        

    }

    public string DialogString(){
        string res = "";
        switch (questState)
        {
            case QuestState.NOT_REQUIREMENTS:
                res += Dialog_1_NOT_REQUIREMENTS();
                break;
            case QuestState.CAN_START:
                res += Dialog_2_CAN_START();
                break;
            case QuestState.IN_PROGRES:
                res += Dialog_3_IN_PROGRES();
                break;
            case QuestState.CAN_FINISH:
                res += Dialog_4_CAN_FINISH();
                break;
            case QuestState.FINISHED:
                res += Dialog_5_FINISHED();
                break;
            case QuestState.FAILED:
                break;
        }
        return res;
    }
     
    // DIALOGE
    protected abstract string Dialog_1_NOT_REQUIREMENTS();
    protected abstract string Dialog_2_CAN_START();
    protected abstract string Dialog_3_IN_PROGRES();
    protected abstract string Dialog_4_CAN_FINISH();
    protected abstract string Dialog_5_FINISHED();


    
    public abstract bool Test_4_CAN_FINISH();


    public abstract void Reward();

}

public enum QuestState{
    NOT_REQUIREMENTS,
    CAN_START,
    IN_PROGRES,
    CAN_FINISH,
    FINISHED,
    FAILED
}
