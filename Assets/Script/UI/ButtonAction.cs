
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
        if (GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandRest();
            GameManager.instance._ExecuteActionQueue();
        }
    }

    public void Wait()
    {
        if (GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandWait();
            GameManager.instance._ExecuteActionQueue();
        }
    }

    public void Talk()
    {
        if (GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandTalk();
            GameManager.instance._ExecuteActionQueue();
        }
    }

    public void Fight()
    {
        if(GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandFight();
            GameManager.instance._ExecuteActionQueue();
        }
    }

    public void Search()
    {
        if(GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandSearch();
            GameManager.instance._ExecuteActionQueue();
        }
    }

    public void Hire()
    {
        if(GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandHire();
            GameManager.instance._ExecuteActionQueue();
        }
    }

    public void Trade()
    {
        if (GameManager.instance.inputBlock == false)
        {
            GameManager.instance.playerCharacter.CommandTrade();
            GameManager.instance._ExecuteActionQueue();
        }
    }

}
