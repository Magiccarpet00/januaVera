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


    public void ToggleFreezeCam()
    {
        if (freezeCam) freezeCam = false;
        else freezeCam = true;
    }

    public void ToggleCamPos()
    {
        Camera cam = Camera.main;

        ToggleFreezeCam();
        if(!CombatManager.instance.GetOnFight())
        {
            transform.position = bufferedPos;
            cam.orthographicSize = Mathf.Clamp(bufferedZoom, clampZoom.x, clampZoom.y);
            cam.rect = new Rect(-0.25f, 0.0f, 1.0f, 1.0f);
            camMapUI.SetActive(true);
        }
        else
        {
            cam.orthographicSize = clampZoom.x;
            Vector3 newPos = new Vector3(CombatManager.instance.GetPosFight().x, CombatManager.instance.GetPosFight().y, -10f);
            cam.transform.SetPositionAndRotation(newPos, Quaternion.identity);
            cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
            camMapUI.SetActive(false);
        }
    }
}
