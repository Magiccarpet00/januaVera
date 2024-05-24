
using TMPro;
using UnityEngine;

public class HireLine : MonoBehaviour
{
    public TextMeshProUGUI nameUI;
    public TextMeshProUGUI priceUI;
    public TextMeshProUGUI timeUI;

    public Character characterToHire;

    public void ClickHire()
    {
        if (GameManager.instance.playerCharacter.HireCharacter(characterToHire))
            Destroy(this.gameObject);
    }
}
