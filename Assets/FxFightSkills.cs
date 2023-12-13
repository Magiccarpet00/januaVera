using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxFightSkills : MonoBehaviour
{
    public void TriggerFxFightSkill(string fxName)
    {
        GetComponent<Animator>().SetTrigger(fxName);
    }
}
