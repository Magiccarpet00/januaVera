using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public GameObject panelInventory;
    public TextMeshProUGUI tmpStatsText;
    public TextMeshProUGUI tmpItemText;
    public bool isOpen;

    private void Awake()
    {
        instance = this;
    }



    private void OpenInventory()
    {
        panelInventory.SetActive(true);
        isOpen = true;
        UpdateInventory();
    }

    private void CloseInventory()
    {
        panelInventory.SetActive(false);
        isOpen = false;
    }

    public void ClickInventoryButton()
    {
        if (isOpen)
            CloseInventory();
        else
            OpenInventory();
    }

    private void UpdateInventory()
    {
        Character playerCharacter = GameManager.instance.playerCharacter;
        string stats = "    STATS\n";
        stats += "VITALITY: " + playerCharacter.c_VITALITY + "/" + playerCharacter.s_VITALITY + "\n";
        stats += "ENDURANCE: " + playerCharacter.c_ENDURANCE + "/" + playerCharacter.c_ENDURANCE + "\n";
        stats += "STRENGHT: " + playerCharacter.c_STRENGHT + "\n";
        stats += "DEXTERITY: " + playerCharacter.c_DEXTERITY + "\n";
        stats += "FAITH: " + playerCharacter.c_FAITH + "\n";
        tmpStatsText.text = stats;

        string objects = "    OBJECTS\n";
        foreach (MyObject obj in playerCharacter.objectInventory)
        {
            objects += "~" + obj.objectData.name + "\n";
        }
        tmpItemText.text = objects;



    }

}
