using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
    public delegate void OnAction();
    public OnAction OnJump;
    public OnAction OnWalkRight;
    public OnAction OnWalkLeft;
    public OnAction OnIdle;
    public OnAction OnLand;
    public OnAction OnEndLand;
    public OnAction OnFall;
    public OnAction OnGetHit;
    public OnAction OnGroundContact;
    public OnAction OnEndGroundContact;

}
