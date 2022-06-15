using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerMovementController : MonoBehaviour
{
    public float Force,JumpForce;
    public Animator Controller;
    public Rigidbody rigidbody;
    public string horizontalAxisName;

    Tween rotateLeftTween, rotateRightTween;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Controller.SetFloat("speed", Mathf.Abs(Input.GetAxis(horizontalAxisName)));
        if(Input.GetAxis(horizontalAxisName) > 0 && rotateLeftTween != null)
        {
            if (rotateLeftTween != null) rotateLeftTween.Kill();
            rotateLeftTween = null;

            rotateRightTween = transform.DORotate(new Vector3(0, 90, 0),0.3f);
        }
        else if (Input.GetAxis(horizontalAxisName) < 0)
        {
            if (rotateRightTween != null) rotateRightTween.Kill();
            rotateRightTween = null;

            rotateLeftTween = transform.DORotate(new Vector3(0, -90, 0), 0.3f);
        }

        float yForce = 0;
        if (Input.GetButtonDown("Jump"))
        {
            Controller.SetBool("jump", true);
            yForce = JumpForce;
        }
        else
        {
            Controller.SetBool("jump", false);
        }
        //TODO check for ground contact

        float calculatedSpeed = Input.GetAxis(horizontalAxisName) * Force;

        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.velocity = new Vector3(calculatedSpeed, yForce, 0);
        }
        else
        {
            rigidbody.velocity = new Vector3(calculatedSpeed, rigidbody.velocity.y, 0);
        }
    }
}
