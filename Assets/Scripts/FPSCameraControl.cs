using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FPSCameraControl : MonoBehaviour
{
    public Camera playerCamera;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float lookYLimit = 80f;
    private float rotationX = 0;
    private float rotationY = 0;

    void Update()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        rotationY += Input.GetAxis("Mouse X") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, -lookYLimit, lookYLimit);
        transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }
}
