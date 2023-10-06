using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatSpot : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI lifeTxt;

    public void UpdateLife()
    {
        lifeTxt.gameObject.SetActive(true);
        lifeTxt.text = character.currentLife + "/" + character.characterData.maxLife;
    }
}
