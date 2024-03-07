using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonItem : MonoBehaviour
{
    public ActiveObject activeObject;

    public void ClickItem()
    {
        switch (activeObject.activeObjectData.skillData.skillType)
        {
            case SkillType.ATTACK:
                break;
            case SkillType.PARRY:
                break;
            case SkillType.SUMMON:
                break;
            case SkillType.HEAL:
                SkillHealData skillHealData = (SkillHealData)activeObject.activeObjectData.skillData;
                GameManager.instance.playerCharacter.TakeHeal(skillHealData.amount);
                break;
        }
        GameManager.instance.playerCharacter.objectInventory.Remove(activeObject);
        InventoryUI.instance.UpdateInventory();
    }
}