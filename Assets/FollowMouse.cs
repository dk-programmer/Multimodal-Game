using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public static FollowMouse instance;
    public LayerMask RaycastLayer;

    private void Start()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, RaycastLayer))
        {
            transform.position = hit.point;
        }
    }
}
