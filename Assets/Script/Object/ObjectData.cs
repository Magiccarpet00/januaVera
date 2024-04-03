using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "JanuaVera/Object Data")]
public class ObjectData : ScriptableObject
{
    public ObjectType objectType;
    public Element material;
    public int init_STATE;

    public int price;
    public string describe;
}
