using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Data/Registerable Gameobject List", order = 1)]
public class Registerable_Gameobject_List_Data : ScriptableObject
{
    public List<GameObject> Reference_List;
    public Actions.OnChangeGameObject OnAddReference,OnRemoveReference;
    public void Register_Gameobject(GameObject go)
    {
        if (!Reference_List.Contains(go))
        {
            OnAddReference?.Invoke(go);
            Reference_List.Add(go);
        }
    }
    public void UnRegister_Gameobject(GameObject go)
    {
        if (Reference_List.Contains(go))
        {
            OnRemoveReference?.Invoke(go);
            Reference_List.Remove(go);
        }
    }
}
