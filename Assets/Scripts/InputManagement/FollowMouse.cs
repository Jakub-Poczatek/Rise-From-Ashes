using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GetMousePosition() != null)
            this.gameObject.transform.Translate((Vector3)GetMousePosition());
    }

    private Vector3? GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3? position = null;
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            position = new Vector3(hit.point.x - transform.position.x, 
                0,
                hit.point.z - transform.position.z);
        }
        //print(position);
        return position;
    }
}
