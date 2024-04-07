using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ButtonSkillTemplateSummon : ButtonSkillTemplate
{
    public TextMeshProUGUI characterToSummon;

    public override void SetUpUI(SkillData skillData)
    {
        base.SetUpUI(skillData);

        SkillSummonData skillSummonData = (SkillSummonData)skillData;
        string strSummonCharacter = "";
        foreach (CharacterData character in skillSummonData.characters)
            strSummonCharacter += character.name + "\n";
        characterToSummon.text = strSummonCharacter;
    }

}
