using UnityEngine;

public class AnimSpriteFight : MonoBehaviour
{
    public SpriteFight spriteFight;

    public void EndAnim()
    {
        spriteFight.AnimDieUI();
    }
}
