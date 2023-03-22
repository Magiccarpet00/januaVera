
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public void Hide()
    {
        if(GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandHide();
            GameManager.instance._ExecuteActionQueue();            
        }
    }

    public void UnHide() //Unhide est une action "gratuite" car elle ne consome pas un tour
    {
        if (GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.UnHide();
            GameManager.instance.UpdateUIButtonGrid(GameManager.instance.playerCharacter.GetCurrentButtonAction());
        }
    }

    public void Rest()
    {
        
    }

    public void Talk()
    {
        
    }

}
