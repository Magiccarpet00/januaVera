using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class ButtonWeapon : MonoBehaviour
{
    public LocalizeStringEvent nameValue;
    public TextMeshProUGUI stateValue;
    public LocalizeStringEvent materialValue;

    public Weapon weapon;
    public bool activeWeapon;

    public void Click()
    {
        if (PlayerCombatManager.instance.inputBlock) return;
        PlayerCombatManager.instance.ClickButtonWeapon(weapon, activeWeapon);
    }

    public void SetUpUI(Weapon w)
    {
        weapon = w;
        WeaponData wd = (WeaponData)w.objectData;
        //LocalizationSettings.StringDatabase.GetLocalizedStringAsync("SWORD").Completed += result => nameValue.text = result;
        nameValue.SetEntry(w.objectData.name);
        stateValue.text = w.currentState.ToString() + "/" + wd.init_STATE.ToString();
        materialValue.SetEntry(w.objectData.material.ToString());
    }

}
