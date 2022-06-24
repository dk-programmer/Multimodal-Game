using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnEnter : EventOnTrigger
{
    public Transform Target;
    public override void Enter(GameObject col, RegisterableCollider.COLLISIONTYPE type)
    {
        base.Enter(col, type);
        RootTransform root = col.GetComponent<RootTransform>();
        if (root != null)
        {
            root.Root.position = Target.position;
        }
    }
}
