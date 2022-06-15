using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventOn : MonoBehaviour
{
    public UnityEvent OnStart_Event, OnAwake_Event, OnDisable_Event, OnEnable_Event;
    public float Delay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDelay(Delay, OnStart_Event));
    }
    private void Awake()
    {
        StartCoroutine(StartDelay(Delay, OnAwake_Event));
    }
    private void OnEnable()
    {
        StartCoroutine(StartDelay(Delay, OnEnable_Event));
    }
    private void OnDisable()
    {
        OnDisable_Event.Invoke();
    }
    public IEnumerator StartDelay(float delay,UnityEvent unityEvent)
    {
        if(delay > 0)
            yield return new WaitForSeconds(delay);
        unityEvent.Invoke();
        yield return null;
    }
}
