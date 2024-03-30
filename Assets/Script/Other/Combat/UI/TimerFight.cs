using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerFight : MonoBehaviour
{
    public TextMeshProUGUI timerTxt;

    public void SetTimer(int time)
    {
        timerTxt.text = time.ToString();
    }

    public void ActiveTimer(bool b)
    {
        timerTxt.enabled = b;
    }
}
