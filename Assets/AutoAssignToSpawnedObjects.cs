using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(+1000)]
public class AutoAssignToSpawnedObjects : MonoBehaviour
{
    public GameObject ObjectToAdd;
    void Start()
    {
        SpawnAreaManager.Instance.AddObjectToSpawnedObjectList(ObjectToAdd);
    }
}
