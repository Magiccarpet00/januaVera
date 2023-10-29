using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CombatSpot : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI lifeTxt;
    public TextMeshProUGUI intentionTxt;

    public void UpdateUI()
    {
        if(lifeTxt.gameObject.activeSelf)
        {
            UpdateLife();
            if (character != GameManager.instance.playerCharacter)
                UpdateIntention();
        }
    }

    public void UpdateIntention()
    {
        intentionTxt.text = character.currentLoadedSkill.damage.ToString() + " " + character.currentLoadedSkill.damageType.ToString() + "\n" + "speed :" + character.currentLoadedSkill.speed.ToString();
    }

    public void UpdateLife()
    {
        lifeTxt.text = character.currentLife + "/" + character.characterData.maxLife;
    }

    public void SetActiveSpotUI(bool b)
    {
        lifeTxt.gameObject.SetActive(b);
        intentionTxt.gameObject.SetActive(b);
    }
}
