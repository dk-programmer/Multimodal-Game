using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registerable_Gameobject_List : MonoBehaviour
{
    public Registerable_Gameobject_List_Data Data;
    private void OnEnable()
    {
        Data.Register_Gameobject(gameObject);
    }
    private void OnDisable()
    {
        Data.UnRegister_Gameobject(gameObject);
    }
}
