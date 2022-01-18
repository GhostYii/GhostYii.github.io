//ORG: Ghostyii & MoonLight Game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraView : MonoBehaviour
{
    public bool lockMouse = true;
    public Texture pointer;
    [Range(0, 100)]
    public float sensitivity = 6f;
    public float minX = -90f;
    public float maxY = 90f;

    private Camera cam;
    private float viewX = 0;
    private float viewY = 0;

    void Start()
    {
        cam = GetComponent<Camera>();
        Cursor.lockState = lockMouse ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update()
    {
        Cursor.lockState = lockMouse ? CursorLockMode.Locked : CursorLockMode.None;

        viewX = Input.GetAxis("Mouse X");

        viewX = cam.transform.localEulerAngles.y + viewX * sensitivity;
        viewY += Input.GetAxis("Mouse Y") * sensitivity;
        viewY = Mathf.Clamp(viewY, minX, maxY);

        cam.transform.localEulerAngles = new Vector3(-viewY, viewX, 0);
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 25, 50, 50), pointer);
    }
}