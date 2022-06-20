using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Transform main;
    public PlayerEvent Events;
    public Animator Controller;

    public int dir = 0;
    public void Start()
    {
        Events.OnWalkLeft += MoveLeft;
        Events.OnWalkRight += MoveRight;
        Events.OnIdle += Idle;
        Events.OnJump += Jump;
        Events.OnFall += Fall;
        Events.OnLand += Land;
        Events.OnGroundContact += GroundContact;
        Events.OnEndGroundContact += EndGroundContact;
    }
    public void Jump()
    {
        Controller.SetTrigger("jump");
    }
    public void GroundContact()
    {
        Controller.SetBool("groundcontact", true);
    }
    public void EndGroundContact()
    {
        Controller.SetBool("groundcontact", false);
    }
    public void Land()
    {
        Controller.SetBool("fall", false);
    }
    public void Fall()
    {
        Controller.SetBool("fall",true);
    }
    public void MoveRight()
    {
        Controller.SetBool("walk", true);
        dir = 1;
    }
    public void MoveLeft()
    {
        Controller.SetBool("walk", true);
        dir = -1;
    }
    public void Idle()
    {
        Controller.SetBool("walk", false);
        dir = 0;
    }
}
