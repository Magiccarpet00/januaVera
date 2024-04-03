using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireUI : MonoBehaviour
{
    public static HireUI instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject panelHire;
    public GameObject gridHire;
    public bool isOpen;

    public GameObject prefabHireLine;
    public List<GameObject> itemHireLine = new List<GameObject>();


    public void OpenHireUI()
    {
        panelHire.SetActive(true);
        isOpen = true;
        GameManager.instance.inputBlock = true;
        GameManager.instance.dialogWindowOpened = true;            
        UpdateHireUI();
    }

    public void CloseHireUI()
    {
        panelHire.SetActive(false);
        isOpen = false;
        GameManager.instance.inputBlock = false;
        GameManager.instance.dialogAnswer = AnswerButton.CLOSE;
        GameManager.instance.dialogWindowOpened = false;
    }

    private void UpdateHireUI()
    {
        foreach (GameObject item in itemHireLine)
            Destroy(item);

        List<Character> charactersInSpot = GameManager.instance.playerCharacter.GetCurrentSpot().GetComponent<Spot>().GetAllCharactersAliveInSpot();
        List<Character> charactersToHire = new List<Character>();
        foreach (Character characterToHire in charactersInSpot)
            if (characterToHire != GameManager.instance.playerCharacter &&
                !GameManager.instance.playerCharacter.followersCharacters.Contains(characterToHire))
                charactersToHire.Add(characterToHire);


        foreach (Character character in charactersToHire)
        {
            GameObject newHireLine = Instantiate(prefabHireLine, transform.position, Quaternion.identity);
            HireLine hireLine = newHireLine.GetComponent<HireLine>();
            newHireLine.transform.SetParent(gridHire.transform);
            newHireLine.transform.localScale = new Vector3(1, 1, 1);

            hireLine.nameUI.text = character.characterData.name;
            hireLine.priceUI.text = character.characterData.workCost.ToString()+" gold";
            hireLine.timeUI.text = "10 turn"; //[CODE WARNING] valeur en dur, dupliqué

            hireLine.characterToHire = character;

            itemHireLine.Add(newHireLine);
        }
    }
}
