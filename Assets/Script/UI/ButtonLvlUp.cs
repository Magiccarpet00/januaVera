using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLvlUp : MonoBehaviour
{
    public Stats statsLvlUp;

    public void Click()
    {
        Character player = GameManager.instance.playerCharacter;
        switch (statsLvlUp)
        {
            case Stats.VITALITY:
                player.s_VITALITY++;
                player.c_VITALITY++;
                break;
            case Stats.ENDURANCE:
                player.s_ENDURANCE++;
                player.c_ENDURANCE++;
                break;
            case Stats.STRENGHT:
                player.s_STRENGHT++;
                player.c_STRENGHT++;
                break;
            case Stats.DEXTERITY:
                player.s_DEXTERITY++;
                player.c_DEXTERITY++;
                break;
            case Stats.FAITH:
                player.s_FAITH++;
                player.c_FAITH++;
                break;
        }
        player.lvlPoint--;
        InventoryUI.instance.UpdateInventory();
    }
}