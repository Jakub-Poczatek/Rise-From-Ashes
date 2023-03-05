using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    private int cameraXMin, cameraXMax, cameraZMin, cameraZMax;
    Vector3? basePointerPosition = null;
    public float cameraMovementSpeed = 0.05f;
    private Camera camera;

    // Rotation
    private bool rotateCamera = false;
    private bool isRotating = false;
    private float rotateAngle = 90;
    private float rotation = 1;
    private float angleCounter = 0;
    private float rotationSpeed = 200;

    // Zoom
    public float maxZoom = 24;
    public float minZoom = 6;
    public float minNearClip = -24;
    public float maxNearClip = -6;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateCamera)
        {
            transform.Rotate(0, (rotation * Time.deltaTime * rotationSpeed), 0);
            angleCounter += 1 * Time.deltaTime * rotationSpeed;
            if (angleCounter >= Mathf.Abs(rotateAngle))
            {
                rotateCamera = false;
                isRotating = false;
            }
        }
    }

    public void MoveCamera(Vector3 pointerPosition)
    {
        if (!basePointerPosition.HasValue)
        {
            basePointerPosition = pointerPosition;
        }
        Vector3 newPosition = pointerPosition - basePointerPosition.Value;
        newPosition = new Vector3(newPosition.x, 0, newPosition.y);
        transform.Translate(newPosition * cameraMovementSpeed);
        LimitPositionInsideCameraBounds();
    }

    public void RotateCamera(float angle)
    {
        if (!isRotating)
        {
            rotateAngle = angle;
            angleCounter = 0;
            rotation = (1 * rotateAngle) / 90;
            isRotating = true;
            rotateCamera = true;
        }
    }

    public void ZoomCamera(float zoom)
    {
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - zoom, minZoom, maxZoom);
        camera.nearClipPlane = Mathf.Clamp(camera.nearClipPlane + zoom, minNearClip, maxNearClip);
    }

    private void LimitPositionInsideCameraBounds()
    {
        transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, cameraXMin, cameraXMax), 
                0,
                Mathf.Clamp(transform.position.z, cameraZMin, cameraZMax)
                );
    }

    public void StopCameraMovement()
    {
        basePointerPosition = null;
    }

    public void SetCameraBounds(int cameraXMin, int cameraXMax, int cameraZMin, int cameraZMax)
    {
        this.cameraXMin = cameraXMin;
        this.cameraXMax = cameraXMax;
        this.cameraZMin = cameraZMin;
        this.cameraZMax = cameraZMax;
    }
}
