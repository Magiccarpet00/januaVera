using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonItem : MonoBehaviour
{
    public MyObject myObject;

    public void ClickItem()
    {
        Character playerCharacter = GameManager.instance.playerCharacter;

        if (myObject.isActiveObject())
        {
            ActiveObject activeObject = (ActiveObject)myObject;
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
                    playerCharacter.TakeHeal(skillHealData.amount);
                    break;
            }
            playerCharacter.objectInventory.Remove(activeObject);
        }

        if (myObject.isArmor())
        {
            playerCharacter.EquipArmor((Armor)myObject);
        }

        if(myObject.isWeapon())
        {
            playerCharacter.EquipWeapon((Weapon)myObject);
        }


        InventoryUI.instance.UpdateInventory();
        GameManager.instance.UpdateTmpInfo();
    }
}