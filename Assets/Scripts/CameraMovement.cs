using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private int cameraXMin, cameraXMax, cameraZMin, cameraZMax;
    Vector3? basePointerPosition = null;
    public float cameraMovementSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
