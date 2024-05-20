using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemShopSell : MonoBehaviour
{
    public TextMeshProUGUI nameUI;
    public TextMeshProUGUI priceUI;

    public MyObject myObject;
    public Character characterMerchant;

    public void ClickSell()
    {
        if (characterMerchant.BuyItem(myObject))
        {
            GameManager.instance.playerCharacter.objectInventory.Remove(myObject);
            GameManager.instance.playerCharacter.gold += myObject.objectData.price;
            ShopUI.instance.UpdateShopUI();
            GameManager.instance.UpdateTmpInfo();
        }
    }
}
