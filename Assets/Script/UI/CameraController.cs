using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool freezeCam;

    public float panSpeed;
    public float scrollSpeed;

    public float panBorder;
    public Vector2 panLimit; //TODO pour ne pas sortir de la map
    public Vector2 clampZoom;

    public Vector3 bufferedPos;
    public float bufferedZoom;

    public GameObject camMapUI;
    public GameObject camFight;

    void Update()
    {   
        if(!freezeCam && !CombatManager.instance.GetOnFight())
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

            Camera cam = Camera.main;

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            bufferedZoom = cam.orthographicSize - (scroll * scrollSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Clamp(bufferedZoom, clampZoom.x, clampZoom.y);

            bufferedPos = pos;
            transform.position = bufferedPos;
        }
    }

    public void ToggleFreezeCam(bool b)
    {
        freezeCam = b;
        //if (freezeCam) freezeCam = false;
        //else freezeCam = true;
    }

    public void ToggleCamPos(bool b)
    {
        Camera cam = Camera.main;

        ToggleFreezeCam(b);


        if (!CombatManager.instance.GetOnFight())
        {
            camMapUI.SetActive(true);
            GameManager.instance.cam_fight.SetActive(false);
        }
        else
        {
            camMapUI.SetActive(false);
            GameManager.instance.cam_fight.SetActive(true);
        }
    }
}
