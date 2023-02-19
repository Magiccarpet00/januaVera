using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject currentSpot;


    //TMP
    private Vector3 target;
    private float smoothTime = 0.2F;
    private Vector3 velocity = Vector3.zero;

    public bool isHero;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }

    public void Move(GameObject spot)
    {
        currentSpot = spot;
        Transform t = currentSpot.transform;

        target = new Vector3(t.position.x, t.position.y, t.position.z);
        //transform.SetPositionAndRotation(v,Quaternion.identity);
    }

    public GameObject GetCurrentSpot()
    {
        return currentSpot;
    }

        

}
