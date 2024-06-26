using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public TextMeshProUGUI nameUI;
    public TextMeshProUGUI priceUI;
    public TextMeshProUGUI descritionUI;

    public MyObject myObject;
    public Character characterMerchant;

    public void ClickBuy()
    {
        if (GameManager.instance.playerCharacter.BuyItem(myObject))
        {
            characterMerchant.gold += myObject.objectData.price;
            characterMerchant.objectToSell.Remove(myObject);
            ShopUI.instance.UpdateShopUI();
            GameManager.instance.UpdateTmpInfo();
        }
    }
}
