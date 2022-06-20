using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// This MonoBehaviour attached to a gameobject with a collider will be able to take suscribtions 
/// </summary>
public class RegisterableCollider : MonoBehaviour
{
    public enum COLLISIONTYPE { ENTER,STAY,EXIT}

    public delegate void CollisionDelegate(GameObject col,COLLISIONTYPE type);
    //Suscribe for Collision;
    public CollisionDelegate Collision_OnEnter, Collision_OnStay, Collision_OnExit;
    //Suscribe for Trigger;
    public CollisionDelegate Trigger_OnEnter, Trigger_OnStay, Trigger_OnExit;

    public List<string> IncludeTag_List, ExcludeTag_List;
    [HorizontalGroup("bools")]
    public bool WaitForExitBeforeRetrigger;
    [HorizontalGroup("bools")]
    public bool CheckForDisabledGO;
    public List<Collider> WaitForExit_List = new List<Collider>();


    public LayerMask CollisionMask;
    private void OnCollisionEnter(Collision collision)
    {
        if (Collision_OnEnter != null && CheckCollisionMask(collision))
        {
            Collision_OnEnter.Invoke(collision.gameObject, COLLISIONTYPE.ENTER);
        }
    }
    private void OnCollisionStay(Collision collision )
    {
        if (Collision_OnStay != null && CheckCollisionMask(collision))
        {
            Collision_OnStay.Invoke(collision.gameObject, COLLISIONTYPE.STAY);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (Collision_OnExit != null && CheckCollisionMask(collision))
        {
            Collision_OnExit.Invoke(collision.gameObject, COLLISIONTYPE.EXIT);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Trigger_OnEnter != null && CheckCollisionMask(other) && CheckTags(other))
        {
            if (WaitForExitBeforeRetrigger)
            {
                AddToWaitingList(other);
                
                if (WaitForExit_List.Count == 1)
                {
                    if (Trigger_OnEnter == null)
                        return;
                    Trigger_OnEnter.Invoke(other.gameObject, COLLISIONTYPE.ENTER);
                }
            }
            else
            {
                if (Trigger_OnEnter == null)
                    return;
                Trigger_OnEnter.Invoke(other.gameObject,COLLISIONTYPE.ENTER);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (Trigger_OnStay != null && CheckCollisionMask(other) && CheckTags(other))
        {
            Trigger_OnStay.Invoke(other.gameObject, COLLISIONTYPE.STAY);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (CheckCollisionMask(other) && CheckTags(other))
        {
            if (WaitForExitBeforeRetrigger)
            {
                RemoveFromWaiting_List(other);
                if (WaitForExit_List.Count == 0)
                {
                    if (Trigger_OnExit == null)
                        return;
                    Trigger_OnExit.Invoke(other.gameObject, COLLISIONTYPE.EXIT);
                }
            }
            else
            {
                if (Trigger_OnExit == null)
                    return;
                Trigger_OnExit.Invoke(other.gameObject, COLLISIONTYPE.EXIT);
            }
        }
    }
    private void Update()
    {
        if (CheckForDisabledGO && WaitForExitBeforeRetrigger)
        {

            Collider last_checked = null;
            bool removedobject = false;
            for (int i = 0; i < WaitForExit_List.Count; i++)
            {
                if (!WaitForExit_List[i] || !WaitForExit_List[i].gameObject.activeInHierarchy || !WaitForExit_List[i].enabled)
                {
                    removedobject = true;
                    last_checked = WaitForExit_List[i];
                    WaitForExit_List.Remove(WaitForExit_List[i]);
                }
            }
            if (WaitForExit_List.Count == 0 && removedobject)
            {
                if (Trigger_OnExit == null)
                    return;
                Trigger_OnExit.Invoke(last_checked.gameObject, COLLISIONTYPE.EXIT);
            }
        }
    }
    /// <summary>
    /// true if collision, false if not
    /// </summary>
    /// <param name="go">Gameobject to be checked</param>
    /// <returns>bool based on input go</returns>
    public bool CheckCollisionMask(Collision go)
    {
        if (CollisionMask == (CollisionMask | (1 << go.gameObject.layer)))
        {
            return true;
        }
        return false;
    }
    public bool CheckTags(Collider go)
    {
        foreach (var item in IncludeTag_List)
        {
            if (go.gameObject.CompareTag(item))
            {
                return true;
            }
        }
        if (IncludeTag_List.Count != 0) return false;//no tag match on a non empty include list
        foreach (var item in ExcludeTag_List)
        {
            if (go.gameObject.CompareTag(item))
            {
                return false;
            }
        }
        return true;//no tag match on a excludelist
    }
    public void AddToWaitingList(Collider go)
    {
        WaitForExit_List.Add(go);
    }
    public void RemoveFromWaiting_List(Collider go)
    {
        WaitForExit_List.Remove(go);
    }
    public bool CheckCollisionMask(Collider go)
    {
        if (!go.isTrigger && CollisionMask == (CollisionMask | (1 << go.gameObject.layer)))
        {
            return true;
        }
        return false;
    }

}
