using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public GameObject panelInventory;
    public TextMeshProUGUI tmpStatsText;
    public TextMeshProUGUI tmpEquipedText;
    public TextMeshProUGUI tmpQuestText;
    public bool isOpen;

    public GameObject panelItemButton;
    public GameObject prefabItemButton;
    public List<GameObject> itemButtons = new List<GameObject>();

    public GameObject panelLvlUp;

    private void Awake()
    {
        instance = this;
    }

    private void OpenInventory()
    {
        panelInventory.SetActive(true);
        isOpen = true;
        GameManager.instance.inputBlock = true;
        UpdateInventory();
    }

    private void CloseInventory()
    {
        panelInventory.SetActive(false);
        isOpen = false;
        GameManager.instance.inputBlock = false;
    }

    public void ClickInventoryButton()
    {
        if (isOpen)
            CloseInventory();
        else
            OpenInventory();
    }

    public void UpdateInventory()
    {

        //-----STAT-----

        foreach (GameObject item in itemButtons)
            Destroy(item);

        panelLvlUp.SetActive(false);

        Character playerCharacter = GameManager.instance.playerCharacter;
        string stats = "    STATS\n\n";
        stats += "LVL " + playerCharacter.lvl + "\n";
        stats += "XP :" + playerCharacter.xp + "\n\n";
        stats += "VITALITY: " + playerCharacter.c_VITALITY + "/" + playerCharacter.s_VITALITY + "\n";
        stats += "ENDURANCE: " + playerCharacter.c_ENDURANCE + "/" + playerCharacter.s_ENDURANCE + "\n";
        stats += "STRENGHT: " + playerCharacter.c_STRENGHT + "\n";
        stats += "DEXTERITY: " + playerCharacter.c_DEXTERITY + "\n";
        stats += "FAITH: " + playerCharacter.c_FAITH + "\n\n";
        if (playerCharacter.lvlPoint > 0)
        {
            stats += "POINT: " + playerCharacter.lvlPoint;
            panelLvlUp.SetActive(true);
        }
        tmpStatsText.text = stats;

        //-----EQUIP-----

        tmpEquipedText.text = "EQUIP:\n\n";
        if (playerCharacter.armorsEquiped.Count > 0)
            tmpEquipedText.text += playerCharacter.armorsEquiped[0].getTmpInfo() +   "\n";
        else
            tmpEquipedText.text += "\n";

        foreach (Weapon weapon in playerCharacter.weaponHands)
            if(weapon !=null)tmpEquipedText.text += weapon.getTmpInfo() + "\n";
            else             tmpEquipedText.text += "------\n";

        //-----QUEST------
        tmpQuestText.text = "QUEST:\n\n";
        foreach (var item in QuestManager.instance.quests)
        {
            if(item.Value.QuestState == QuestState.IN_PROGRES)
                tmpQuestText.text += item.Value.questName + " " + item.Value.nbTurn + "/" + item.Value.nbTurnMax + " turn\n";
        }

        //-----INVENTORY-----
        foreach (MyObject item in playerCharacter.objectInventory)
        {
            if(item.c_STATE > 0 && playerCharacter.isEquipedObject(item) == false)
            {
                if (item.isActiveObject() || item.isArmor() || item.isWeapon())
                {
                    GameObject newItemButton = Instantiate(prefabItemButton, transform.position, Quaternion.identity);
                    newItemButton.GetComponentInChildren<TextMeshProUGUI>().text = item.objectData.name;
                    newItemButton.transform.SetParent(panelItemButton.transform);
                    newItemButton.transform.localScale = new Vector3(1, 1, 1);

                    newItemButton.GetComponent<ButtonItem>().myObject = item;

                    itemButtons.Add(newItemButton);
                }
                else
                {
                    GameObject newItemButton = Instantiate(prefabItemButton, transform.position, Quaternion.identity);
                    newItemButton.GetComponentInChildren<TextMeshProUGUI>().text = item.objectData.name;
                    newItemButton.transform.SetParent(panelItemButton.transform);
                    newItemButton.transform.localScale = new Vector3(1, 1, 1);

                    newItemButton.GetComponent<Button>().interactable = false;

                    itemButtons.Add(newItemButton);
                }
            }
            else
            {
                //playerCharacter.objectInventory.Remove(item); //[CODE CARNAGE] Vraiment pas du tout à sa place
            }
        }
    }

}
