using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public Player playerCharacter;
    public GameObject panelInventory;
    public bool isOpen;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerCharacter = GameManager.instance.playerCharacter;
    }

    private void OpenInventory()
    {
        panelInventory.SetActive(true);
        isOpen = true;
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

}
