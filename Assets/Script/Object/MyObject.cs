using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyObject
{
    public ObjectData objectData;
    public int c_STATE;

    public MyObject(ObjectData objData)
    {
        objectData = objData;
        c_STATE = objectData.init_STATE;
    }

    public virtual void UseObject() {;}

    public virtual bool isWeapon() { return false; }
    public virtual bool isArmor() { return false; }
    public virtual bool isActiveObject() { return false; }

    
    
}
