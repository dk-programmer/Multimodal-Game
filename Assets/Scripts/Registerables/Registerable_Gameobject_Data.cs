using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Registerable Gameobject", order = 1)]
public class Registerable_Gameobject_Data : ScriptableObject
{
    public GameObject Reference;
    public Actions.OnChangeGameObject OnChangeReference;
    public void Register_Gameobject(GameObject go)
    {
        OnChangeReference.Invoke(go);
        Reference = go;
    }
    public void UnRegister_Gameobject(GameObject go)
    {
        if (Reference == go)
        {
            OnChangeReference.Invoke(null);
            Reference = null;
        }
    }
}
