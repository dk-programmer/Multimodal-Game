using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform Target;
    public Transform Move;
    private void LateUpdate()
    {
        Move.position = Target.position;
    }
}
