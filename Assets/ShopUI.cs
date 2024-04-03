using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject panelShop;
    public GameObject gridShop;
    public GameObject gridInventory;
    public bool isOpen;

    public GameObject prefabShopItem;
    public GameObject prefabItemInventory;

    public List<GameObject> itemShopItemUI = new List<GameObject>();
    public List<GameObject> itemItemInventoryUI = new List<GameObject>();

    public Character characterMerchant;
    public TextMeshProUGUI merchantGold;

    public void OpenShopUI()
    {
        panelShop.SetActive(true);
        isOpen = true;
        GameManager.instance.inputBlock = true;
        GameManager.instance.dialogWindowOpened = true;
        UpdateShopUI();
    }

    public void CloseShopUI()
    {
        panelShop.SetActive(false);
        isOpen = false;
        GameManager.instance.inputBlock = false;
        GameManager.instance.dialogAnswer = AnswerButton.CLOSE;
        GameManager.instance.dialogWindowOpened = false;
    }

    public void UpdateShopUI()
    {
        foreach (GameObject item in itemShopItemUI)
            Destroy(item);

        foreach (GameObject item in itemItemInventoryUI)
            Destroy(item);

        //[CODE WARNING-TODO] Ne marche pas si il ya deux shop sur la meme case pour l'instant
        List<Character> charactersInSpot = GameManager.instance.playerCharacter.GetCurrentSpot().GetComponent<Spot>().GetAllCharactersAliveInSpot();
        foreach (Character character in charactersInSpot)
        {
            if (character.characterData.isMerchant)
            {
                characterMerchant = character;
                break;
            }
        }

        if (characterMerchant == null)
            Debug.LogWarning("no characterMerchant on this spot");

        merchantGold.text = "Marchant's gold: " + characterMerchant.gold + "g";

        foreach (MyObject myObject in characterMerchant.objectToSell)
        {
            GameObject newItemShop = Instantiate(prefabShopItem, transform.position, Quaternion.identity);
            ItemShop itemShop = newItemShop.GetComponent<ItemShop>();
            newItemShop.transform.SetParent(gridShop.transform);
            newItemShop.transform.localScale = new Vector3(1, 1, 1);

            itemShop.nameUI.text = myObject.objectData.name;
            itemShop.priceUI.text = myObject.objectData.price.ToString()+ "gold";
            itemShop.descritionUI.text = myObject.objectData.describe;

            itemShop.myObject = myObject;
            itemShop.characterMerchant = characterMerchant;

            itemShopItemUI.Add(newItemShop);
        }

        foreach (MyObject myObject in GameManager.instance.playerCharacter.objectInventory)
        {
            GameObject newItem = Instantiate(prefabItemInventory, transform.position, Quaternion.identity);
            ItemShopSell itemShopSell = newItem.GetComponent<ItemShopSell>();
            newItem.transform.SetParent(gridInventory.transform);
            newItem.transform.localScale = new Vector3(1, 1, 1);

            itemShopSell.nameUI.text = myObject.objectData.name;
            itemShopSell.priceUI.text = myObject.objectData.price.ToString() + "gold";

            itemShopSell.myObject = myObject;
            itemShopSell.characterMerchant = characterMerchant;

            itemItemInventoryUI.Add(newItem);
        }
    }
}
