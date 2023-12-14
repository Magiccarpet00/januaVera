using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class ButtonWeapon : MonoBehaviour
{
    public LocalizeStringEvent nameValue;

    public LocalizeStringEvent styleValue;
    public TextMeshProUGUI stateValue;
    public LocalizeStringEvent materialValue;

    public Weapon weapon;

    public void Click()
    {
        CombatManager.instance.ClickButtonWeapon(weapon);
    }

    public void SetUpUI(Weapon w)
    {
        weapon = w;
        //LocalizationSettings.StringDatabase.GetLocalizedStringAsync("SWORD").Completed += result => nameValue.text = result;

        nameValue.SetEntry(w.weaponData.name);
        styleValue.SetEntry(w.weaponData.style.ToString()); //TODO recfactot nommage style/type
        stateValue.text = w.currentState.ToString() + "/" + weapon.weaponData.maxState.ToString();
        materialValue.SetEntry(w.weaponData.material.ToString());
    }

}
