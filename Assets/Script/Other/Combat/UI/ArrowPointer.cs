using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public GameObject prefabPointerCircle;
    public int nbCircles;

    public List<GameObject> circles = new List<GameObject>();
    public Transform player;
    private Vector3 worldPosition;

    private Camera cameraFight;

    void Start()
    {
        cameraFight = GameManager.instance.cam_fight.GetComponent<Camera>();


        for (int i = 0; i < nbCircles; i++)
        {
            circles.Add(Instantiate(prefabPointerCircle, transform.position, Quaternion.identity));
        }

    }

    
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cameraFight.nearClipPlane;
        worldPosition = cameraFight.ScreenToWorldPoint(mousePos);

        //circles[0].transform.position.Set(worldPosition.x, worldPosition.y, 0f);
        
        for (int i = 0; i < nbCircles; i++)
        {
            float lerp_x = Mathf.Lerp(-14, worldPosition.x, i/nbCircles);
            float lerp_y = Mathf.Lerp(-30, worldPosition.y, i/nbCircles);

            circles[i].transform.SetPositionAndRotation(new Vector3(lerp_x, lerp_y, 0f), Quaternion.identity);
        }
    }
}
