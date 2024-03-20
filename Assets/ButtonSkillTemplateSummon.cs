using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateSummon : MonoBehaviour
{
    public LocalizeStringEvent nameValue;
    public TextMeshProUGUI speedValue;
    public TextMeshProUGUI characterToSummon;

    public void SetUpUI(SkillData skillData)
    {
        SkillSummonData skillSummonData = (SkillSummonData)skillData;
        nameValue.SetEntry(skillSummonData.name);
        speedValue.text = skillSummonData.speed.ToString();

        string strSummonCharacter = "";
        foreach (CharacterData character in skillSummonData.characters)
            strSummonCharacter += character.name + "\n";
        characterToSummon.text = strSummonCharacter;
    }

}
