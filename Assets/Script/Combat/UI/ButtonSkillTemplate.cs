using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ButtonSkillTemplate : MonoBehaviour
{
    public LocalizeStringEvent nameValue;
    public TextMeshProUGUI speedValue;
    public TextMeshProUGUI requirementValue;

    public virtual void SetUpUI(SkillData skillData)
    {
        nameValue.SetEntry(skillData.name);
        speedValue.text = skillData.speed.ToString();
        //TMP
        string reqValue = "";
        if (skillData.req_VITALITY != 0)
            reqValue += " VIT >= " + skillData.req_VITALITY + " ";
        if (skillData.req_ENDURANCE != 0)
            reqValue += " END >= " + skillData.req_ENDURANCE + " ";
        if (skillData.req_FAITH != 0)
            reqValue += " FAI >= " + skillData.req_FAITH + " ";
        if (skillData.req_STRENGHT != 0)
            reqValue += " STR >= " + skillData.req_STRENGHT + " ";
        if (skillData.req_DEXTERITY != 0)
            reqValue += " DEX >= " + skillData.req_DEXTERITY + " ";
        requirementValue.text = reqValue;

      
    }
}
