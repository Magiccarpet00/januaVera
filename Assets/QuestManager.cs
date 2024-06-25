using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    //public GameObject[] allQuest;

    public List<Quest> quests = new List<Quest>();

    private void Awake()
    {
        instance = this;
    }

    //private void Start()
    //{
    //    foreach (GameObject quest in allQuest)
    //        quests.Add(quest.GetComponent<Quest>());
    //}

    public void AddQuest(GameObject quest)
    {
        GameObject gameObjectQuest = Instantiate(quest);
        gameObjectQuest.transform.parent = transform;
        quests.Add(gameObjectQuest.GetComponent<Quest>());

        //TODO REPRENDRE ICI
    }

    public void UpdateQuests()
    {
        foreach (Quest quest in quests)
        {
            quest.UpdateState();
        }
    }

}
