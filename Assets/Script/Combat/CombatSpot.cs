using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatSpot : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI lifeTxt;

    public void UpdateUI()
    {
        if(lifeTxt.gameObject.activeSelf)
            UpdateLife();
    }

    public void UpdateLife()
    {
        lifeTxt.text = character.currentLife + "/" + character.characterData.maxLife;
    }

    public void SetActiveLifeText(bool b)
    {
        lifeTxt.gameObject.SetActive(b);
    }
}
