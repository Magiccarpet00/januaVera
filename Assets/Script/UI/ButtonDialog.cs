using UnityEngine;

public class ButtonDialog : MonoBehaviour
{
    public AnswerButton dialogAnswer;

    public void ClickDialog()
    {
        GameManager.instance.dialogAnswer = dialogAnswer;
    }
}
