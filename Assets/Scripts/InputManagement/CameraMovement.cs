using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    private Camera childCamera;

    // Move
    private int cameraXMin, cameraXMax, cameraZMin, cameraZMax;
    Vector3? basePointerPosition = null;
    public float cameraPanSpeed = 0.05f;
    public float cameraMoveSpeed = 1;

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

    private CameraMovement() {}

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public static CameraMovement Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        childCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotate();
    }

    private void UpdateRotate()
    {
        if (rotateCamera)
        {
            transform.Rotate(0, (rotation * Time.deltaTime * rotationSpeed), 0);
            angleCounter += 1 * Time.deltaTime * rotationSpeed;
            if (angleCounter >= Mathf.Abs(rotateAngle))
            {
                transform.rotation = 
                    Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Round(transform.rotation.eulerAngles.y), transform.rotation.eulerAngles.z);
                rotateCamera = false;
                isRotating = false;
            }
        }
    }

    public void PanCamera(Vector3 pointerPosition)
    {
        if (!basePointerPosition.HasValue)
        {
            basePointerPosition = pointerPosition;
        }
        Vector3 newPosition = pointerPosition - basePointerPosition.Value;
        newPosition = new Vector3(newPosition.x, 0, newPosition.y);
        transform.Translate(newPosition * Time.deltaTime * cameraPanSpeed);
        ClampPosition();
    }

    public void SnapCamera(Vector3 position)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }

    public void MoveCamera(Vector2 direction)
    {
        Vector3 moveBy = new Vector3(direction.x, 0, direction.y);
        transform.Translate(moveBy * cameraMoveSpeed);
        ClampPosition();
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
        childCamera.orthographicSize -= zoom;
        childCamera.nearClipPlane += zoom;
        ClampOrthographicSizeAndNearClipPlane();
    }

    private void ClampPosition()
    {
        transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, cameraXMin, cameraXMax), 
                0,
                Mathf.Clamp(transform.position.z, cameraZMin, cameraZMax)
                );
    }

    private void ClampOrthographicSizeAndNearClipPlane()
    {
        childCamera.orthographicSize = Mathf.Clamp(childCamera.orthographicSize, minZoom, maxZoom);
        childCamera.nearClipPlane = Mathf.Clamp(childCamera.nearClipPlane, minNearClip, maxNearClip);
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
