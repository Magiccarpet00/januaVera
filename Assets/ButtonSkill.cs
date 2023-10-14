using TMPro;
using UnityEngine;

public class ButtonSkill : MonoBehaviour
{
    public TextMeshProUGUI btnName;
    public TextMeshProUGUI btnText;

    public SkillData skillData;

    public void Click()
    {
        CombatManager.instance.ClickButtonSkill(skillData);
    }
}
