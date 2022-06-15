using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions
{
    public enum TRIGGERTYPE { ENTER,STAY,EXIT };

    public delegate void OnAction();
    public delegate void OnChangeFloat(float value);
    public delegate void OnChangeVector3(Vector3 value);
    public delegate void OnChangeInt(int value);
    public delegate void OnChangeGameObject(GameObject value);
    public delegate void OnVectorGamobjectEvent(Vector3 pos,GameObject go);
    public delegate void OnTriggerEnterStayExitEvent(GameObject trigger,GameObject caller,TRIGGERTYPE triggertype);
}
