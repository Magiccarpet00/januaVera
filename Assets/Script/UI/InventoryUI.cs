using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public GameObject panelInventory;
    public TextMeshProUGUI tmpStatsText;
    public bool isOpen;

    public GameObject panelItemButton;
    public GameObject prefabItemButton;
    public List<GameObject> itemButtons = new List<GameObject>();


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
        foreach (GameObject item in itemButtons)
            Destroy(item);

        Character playerCharacter = GameManager.instance.playerCharacter;
        string stats = "    STATS\n";
        stats += "VITALITY: " + playerCharacter.c_VITALITY + "/" + playerCharacter.s_VITALITY + "\n";
        stats += "ENDURANCE: " + playerCharacter.c_ENDURANCE + "/" + playerCharacter.c_ENDURANCE + "\n";
        stats += "STRENGHT: " + playerCharacter.c_STRENGHT + "\n";
        stats += "DEXTERITY: " + playerCharacter.c_DEXTERITY + "\n";
        stats += "FAITH: " + playerCharacter.c_FAITH + "\n";
        tmpStatsText.text = stats;

        foreach (ActiveObject item in playerCharacter.objectInventory)
        {
            GameObject newItemButton = Instantiate(prefabItemButton, transform.position, Quaternion.identity);
            newItemButton.GetComponentInChildren<TextMeshProUGUI>().text = item.objectData.name;
            newItemButton.transform.SetParent(panelItemButton.transform);
            newItemButton.transform.localScale = new Vector3(1, 1, 1);

            newItemButton.GetComponent<ButtonItem>().activeObject = item;

            itemButtons.Add(newItemButton);
        }
    }

}
