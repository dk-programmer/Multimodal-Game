using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterableGameobject : MonoBehaviour
{
    public Registerable_Gameobject_Data Data;
    private void OnEnable()
    {
        Data.Reference = gameObject;
    }
    private void OnDisable()
    {
        if (Data.Reference == gameObject) Data.Reference = null;
    }
}
