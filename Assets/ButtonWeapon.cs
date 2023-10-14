using TMPro;
using UnityEngine;

public class ButtonWeapon : MonoBehaviour
{
    public TextMeshProUGUI btnName;
    public TextMeshProUGUI btnStyle;
    public TextMeshProUGUI btnState;
    public TextMeshProUGUI btnMaterial;

    public Weapon weapon;

    public void Click()
    {
        CombatManager.instance.ClickButtonWeapon(weapon);
    }

}
