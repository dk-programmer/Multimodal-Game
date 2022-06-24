using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static RegisterableCollider;

public class EventOnTrigger : MonoBehaviour
{
    public UnityEvent OnEnter, OnStay, OnExit;
    public RegisterableCollider Collider;
    // Start is called before the first frame update
    void Start()
    {
        Collider.Trigger_OnEnter += Enter;
        Collider.Trigger_OnStay += Stay;
        Collider.Trigger_OnExit += Exit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Enter(GameObject col, COLLISIONTYPE type)
    {
        OnEnter?.Invoke();
    }
    public virtual void Stay(GameObject col, COLLISIONTYPE type)
    {
        OnStay?.Invoke();
    }
    public virtual void Exit(GameObject col, COLLISIONTYPE type)
    {
        OnExit?.Invoke();
    }
}
