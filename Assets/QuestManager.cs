using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    //public GameObject[] allQuest;
    public int idCount = 0;
    public Dictionary<string,Quest> quests = new Dictionary<string, Quest>();

    private void Awake()
    {
        instance = this;
    }

    //private void Start()
    //{
    //    foreach (GameObject quest in allQuest)
    //        quests.Add(quest.GetComponent<Quest>());
    //}

    public void AddQuest(Character questGiver,GameObject quest)
    {
        GameObject gameObjectQuest = Instantiate(quest);
        gameObjectQuest.transform.parent = transform;

        Quest _quest = gameObjectQuest.GetComponent<Quest>();
        _quest.questGiver = questGiver;

        quests.Add(questGiver.name, _quest);
        
        gameObjectQuest.name = questGiver.name;

        //TODO REPRENDRE ICI
    }

    public void UpdateQuests()
    {
        foreach (var quest in quests)
        {
            quest.Value.UpdateState();
        }
    }

}
