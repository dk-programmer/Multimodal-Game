using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RegisterableCollider;
using DG.Tweening;
using Rewired;
using Sirenix.OdinInspector;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{
    public static List<PlayerMovement> instances = new  List<PlayerMovement>(); 
    [Header("reference")]
    //reference
    public Player p;
    public PlayerEvent p_events;
    public RegisterableCollider GroundCheck_Trigger,Head_Trigger,BodyCollider;
    public Rigidbody Rigid;
    [Header("Settings")]
    //settings
    public float Playerspeed = 1;
    public AnimationCurve JumpStrength;
    public float jumpstrength = 1;
    public float MaxJumpTime = 0.5f;

    //temps
    private int dir = 0;
    private int look_dir = 1;
    private bool Jumping;
    private bool GroundContact;

    public bool UseVoiceInput;
    public float WalkControlValue;
    public bool JumpControlValue;

    private bool blockDamage;

    private Coroutine jumpRoutine;
    private Tween rotationtween;
    private void OnDisable()
    {
        instances.Remove(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        p = ReInput.players.GetPlayer(0);
        instances.Add(this);
        p_events.OnJump += Jump;
        p_events.OnWalkRight += OnWalkRight;
        p_events.OnWalkLeft += OnWalkLeft;

        GroundCheck_Trigger.Trigger_OnEnter += OnGroundEnter;
        GroundCheck_Trigger.Trigger_OnStay += OnGroundContact;
        GroundCheck_Trigger.Trigger_OnExit += OnNoGroundContact;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(UseVoiceInput)
        {
            if (p.GetAnyNegativeButton() || p.GetAnyButton())
            {
                UseVoiceInput = false;
            }
        }
        if(!UseVoiceInput)
        {
            WalkControlValue = p.GetAxis("walk");
            JumpControlValue = p.GetButtonDown("jump");
        }

        if (WalkControlValue == 0 && dir != 0)
        {
            p_events.OnIdle?.Invoke();
            dir = 0;
        }
        else if (WalkControlValue > 0)
        {
            if(dir != 1)
            {
                if(rotationtween != null)
                {
                    rotationtween.Kill();
                    rotationtween = null;
                }
                rotationtween = transform.DORotate(new Vector3(0,90,0),0.3f);
            }
            dir = 1;
            look_dir = 1;
            p_events.OnWalkRight?.Invoke();
        }
        else if (WalkControlValue < 0)
        {
            if (dir != -1)
            {
                if (rotationtween != null)
                {
                    rotationtween.Kill();
                    rotationtween = null;
                }
                rotationtween = transform.DORotate(new Vector3(0, -90, 0), 0.3f);
            }
            dir = -1;
            look_dir = -1;
            p_events.OnWalkLeft?.Invoke();
        }
        
        if(dir == 0 && !blockDamage)
        {
            var velocity_x = Rigid.velocity.x;
            var target_velocity_x = (1 * dir * Playerspeed) - velocity_x;
            Rigid.AddForce(new Vector3(target_velocity_x, 0, 0), ForceMode.VelocityChange);
        }
        else if (dir != 0 && !blockDamage)
        {
            var velocity_x = Rigid.velocity.x;
            var target_velocity_x = (1 * dir * Playerspeed) - velocity_x;
            Rigid.AddForce(new Vector3(target_velocity_x,0,0), ForceMode.VelocityChange);
        }

        if (JumpControlValue)
        {
            JumpControlValue = false;
            if (Jumping) return;
            p_events.OnJump?.Invoke();
        }
    }
    public void OnWalkLeft()
    {
        transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    }
    public void OnWalkRight()
    {
        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
    }
    public void OnGroundEnter(GameObject col, COLLISIONTYPE type)
    {
        RootTransform r = col.GetComponent<RootTransform>();
        p_events.OnGroundContact?.Invoke();
        if (r != null)
        {
            IDamageable dam = r.Root.GetComponent<IDamageable>();
            if (dam != null)
            {
                Jumping = false;
                GroundContact = true;
                Jump();
                dam.GetHit(1);
            }
        }
    }
    public void OnGroundContact(GameObject col, COLLISIONTYPE type)
    {
        if (blockDamage || GroundContact)
        {
            return;
        }
        GroundContact = true;
        OnLand();
    }
    public void OnLand()
    {
        p_events.OnLand?.Invoke();
        Invoke("OnEndLand", 0.1f);
    }
    public void OnEndLand()
    {
        p_events.OnEndLand?.Invoke();
    }
    public void OnNoGroundContact(GameObject col, COLLISIONTYPE type)
    {
        GroundContact = false;
        p_events.OnEndGroundContact?.Invoke();
    }
    public void Jump()
    {
        if (Jumping || !GroundContact) return;
        Jumping = true;

        if (jumpRoutine != null) StopCoroutine(jumpRoutine);
        jumpRoutine = StartCoroutine(StartJumpRoutine());
    }
    public void EndJump()
    {
        if (jumpRoutine != null) StopCoroutine(jumpRoutine);
        Jumping = false ;
    }
    public IEnumerator StartJumpRoutine()
    {
        float Timer = 0;
        float gravitivmultiply = 2;
        
        yield return new WaitForFixedUpdate();
        var velocity_y = Rigid.velocity.y;
        var target_velocity_y = (1 * jumpstrength) - velocity_y;
        Rigid.AddForce(new Vector3(0, target_velocity_y, 0), ForceMode.VelocityChange);
        while ((UseVoiceInput || p.GetButton("Jump")) && Timer < MaxJumpTime)
        {
            velocity_y = Rigid.velocity.y;
            target_velocity_y = (1 * jumpstrength) - velocity_y;
            Rigid.AddForce(new Vector3(0, target_velocity_y, 0), ForceMode.VelocityChange);
            Timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        p_events.OnFall?.Invoke();
        while (!GroundContact)
        {
            Rigid.AddForce(Physics.gravity.y * Vector3.up * Time.fixedDeltaTime * (gravitivmultiply-1));
            yield return new WaitForFixedUpdate();
        }
        EndJump();
    }

    public void GetHit()
    {
        if (blockDamage) return;
        blockDamage = true;
        OnNoGroundContact(null,0);
        p_events.OnFall?.Invoke();
        EndJump();
        var velocity_y = Rigid.velocity.y;
        var target_velocity_y = (1 * jumpstrength) - velocity_y;

        var velocity_x = Rigid.velocity.x;
        var target_velocity_x = (-1 * look_dir * jumpstrength) - velocity_x;

        Rigid.AddForce(new Vector3(target_velocity_x, target_velocity_y, 0), ForceMode.VelocityChange);

        p_events.OnGetHit?.Invoke();
        Invoke("EndGetHit",1f);
    }
    public void EndGetHit()
    {
        blockDamage = false;
    }
}
