using UnityEngine;

public class ButtonDialog : MonoBehaviour
{
    public DialogAnswer dialogAnswer;

    public void ClickDialog()
    {
        GameManager.instance.dialogAnswer = dialogAnswer;
    }
}
