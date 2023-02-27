using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool freezeCam;

    public float panSpeed;
    public float scrollSpeed;

    public float panBorder;
    public Vector2 panLimit; //todo pour ne pas sortir de la map
    public Vector2 clampZoom;
    void Update()
    {   
        if(!freezeCam)
        {
            Vector3 pos = transform.position;

            if (Input.mousePosition.y >= Screen.height - panBorder)
            {
                pos.y += panSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y <= panBorder)
            {
                pos.y -= panSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.x >= Screen.width - panBorder)
            {
                pos.x += panSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.x <= panBorder)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }

            //todo fini avec le tuto
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Camera cam = Camera.main;
            float camSize = cam.orthographicSize - ( scroll * scrollSpeed * Time.deltaTime);

            cam.orthographicSize = Mathf.Clamp(camSize, clampZoom.x, clampZoom.y);
            

            transform.position = pos;
        }
    }
}
