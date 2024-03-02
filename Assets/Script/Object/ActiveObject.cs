using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : MyObject
{
    public ActiveObjectData activeObjectData;
    

    public ActiveObject(ActiveObjectData aod) : base(aod)
    {
        activeObjectData = aod;
    }

    public override bool isActiveObject() { return true; }
    
}
