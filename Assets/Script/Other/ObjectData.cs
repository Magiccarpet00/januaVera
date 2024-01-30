using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "JanuaVera/Object Data")]
public class ObjectData : ScriptableObject
{
    public Element material;
    public int maxState;
}
