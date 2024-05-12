using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableManager : MonoBehaviour
{
    //CODE PLACARD
    //Ce code n'est plus vraiment utile

    public static ScriptableManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<ScriptableObject> scriptableObjects = new List<ScriptableObject>();

    public ScriptableObject FindData(string name) // Il faut la cast avant appelle
    {
        foreach (ScriptableObject data in scriptableObjects)
        {
            if (data.name.Equals(name))
                return data;
        }

        Debug.LogError("DATA NOT FIND in ScriptableManager = " + name);
        return null;
    }
}
