using System.Collections;
using System.Collections.Generic;
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
    public bool isOpen;

    public GameObject prefabShopItem;
    public List<GameObject> itemShopItemUI = new List<GameObject>();

    public Character characterMerchant;

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

    private void UpdateShopUI()
    {
        foreach (GameObject item in itemShopItemUI)
            Destroy(item);

        //[CODE WARNING-TODO] Ne marche pas si il ya deux shop sur la meme case pour l'instant
        List<Character> charactersInSpot = GameManager.instance.playerCharacter.GetAllCharactersAliveInSpot();
        foreach (Character character in charactersInSpot)
        {
            if (character.isMerchant)
            {
                characterMerchant = character;
                break;
            }
        }

        if (characterMerchant == null)
            Debug.LogWarning("no characterMerchant on this spot");

        foreach (MyObject myObject in characterMerchant.objectToSell)
        {
            GameObject newItemShop = Instantiate(prefabShopItem, transform.position, Quaternion.identity);
            ItemShop itemShop = newItemShop.GetComponent<ItemShop>();
            newItemShop.transform.SetParent(gridShop.transform);
            newItemShop.transform.localScale = new Vector3(1, 1, 1);

            itemShop.nameUI.text = myObject.objectData.name;
            itemShop.priceUI.text = myObject.objectData.price.ToString();
            itemShop.descritionUI.text = myObject.objectData.describe;

            itemShop.myObject = myObject;
            itemShop.characterMerchant = characterMerchant;

            itemShopItemUI.Add(newItemShop);
        }
    }
}
