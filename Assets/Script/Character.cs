using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject currentSpot;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move(GameObject spot)
    {
        currentSpot = spot;
        Transform t = currentSpot.transform;

        Debug.Log(t.position.x + " " + t.position.y + " " + t.position.z);

        Vector3 v = new Vector3(t.position.x, t.position.y, t.position.z);

        transform.SetPositionAndRotation(v,Quaternion.identity);
    }

    public GameObject GetCurrentSpot()
    {
        return currentSpot;
    }

        

}
