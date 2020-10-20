using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed;
    public float zoomSpeed;
    public float maxSize;
    public Camera cam;

    public GUIMaster gui;

    private Vector3 lastMousePos;

    private void Update()
    {
        if (gui.GUIState.CameraControl)
        {
            float dx = Input.GetAxis("Horizontal") * cam.orthographicSize;
            float dy = Input.GetAxis("Vertical") * cam.orthographicSize;
            if (Mathf.Abs(dx) > 0.1 || Mathf.Abs(dy) > 0.1)
            {
                transform.position += new Vector3(dx * movementSpeed * Time.deltaTime, dy * movementSpeed * Time.deltaTime, 0);
            }

            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            cam.orthographicSize = Mathf.Min(Mathf.Max(cam.orthographicSize, 1), maxSize);

            if (Input.GetMouseButtonDown(2))
            {
                lastMousePos = Input.mousePosition;
            }

            else if (Input.GetMouseButton(2))
            {
                transform.position += cam.ScreenToWorldPoint(lastMousePos) - cam.ScreenToWorldPoint(Input.mousePosition);
                lastMousePos = Input.mousePosition;
            }
        }
    }

}
