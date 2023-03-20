
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public void Hide()
    {
        if(GameManager.instance.inputBlock == false)
        {
            GameManager.instance.player.GetComponent<Character>().CommandHide();
            StartCoroutine(GameManager.instance.ExecuteActionQueue());
        }
    }

    public void Rest()
    {
        
    }

    public void Talk()
    {
        
    }

}
