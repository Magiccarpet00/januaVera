using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool freezeCam;

    public float panSpeed;
    public float scrollSpeed;

    public float panBorder;
    public float limitUp;
    public float limitRight;
    public float limitDown;
    public float limitLeft;
    public Vector2 clampZoom;

    public Vector3 bufferedPos;
    public float bufferedZoom;

    public GameObject camMapUI;
    public GameObject camFight;

    void FixedUpdate()
    {   
        if(!freezeCam && GameManager.instance.playerCharacter?.onFight == false)
        {
            Vector3 pos = transform.position;

            if (Input.mousePosition.y >= Screen.height - panBorder && transform.position.y < limitUp)
                pos.y += panSpeed * Time.deltaTime;

            if (Input.mousePosition.y <= panBorder && transform.position.y > limitDown)
                pos.y -= panSpeed * Time.deltaTime;

            if (Input.mousePosition.x >= Screen.width - panBorder && transform.position.x < limitRight)
                pos.x += panSpeed * Time.deltaTime;

            if (Input.mousePosition.x <= panBorder && transform.position.x > limitLeft)
                pos.x -= panSpeed * Time.deltaTime;

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
    }

    public void ToggleCamPos(bool b)
    {
        Camera cam = Camera.main;

        ToggleFreezeCam(b);


        if (!GameManager.instance.playerCharacter.onFight)
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
