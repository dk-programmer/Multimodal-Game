using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SpawnAreaManager : MonoBehaviour
{
    public static SpawnAreaManager Instance;
    public List<PrefabContainer> PrefabList;
    List<GameObject> SpawnedObjectList = new List<GameObject>();
    public Transform MouseTransform;
    public Transform PrefabSpawnRoot;
    public float MaxDeleteRange = 15;
    private void Start()
    {
        Instance = this;
    }

    public void AddObjectToSpawnedObjectList(GameObject go)
    {
        SpawnedObjectList.Add(go);
    }

    public void CreateObject(string text)
    {
        GameObject prefab = GetPrefab(text);
        GameObject go = Instantiate(prefab, PrefabSpawnRoot);
        SpawnedObjectList.Add(go);
        if (text.Contains("there"))
        {
            go.transform.position = GetPositionNearMouse();
        }
        else
        {
            go.transform.position = GetPositionNearPlayer();
        }
    }
    public void DeleteObject(string text)
    {
        GameObject go;
        if (text.Contains("that"))
        {
            go = GetObjectNearMouse(FilterName(text));
        }
        else
        {
            go = GetObjectNearPlayer(FilterName(text));
        }
        if (Vector3.Distance(go.transform.position, PlayerMovement.instances[0].transform.position) > MaxDeleteRange) return;
        if (go == null) return;
        go.transform.DOScale(Vector3.zero, 0.3f);
        SpawnedObjectList.Remove(go);
        Destroy(go, 1f);
    }


    public Vector3 GetPositionNearPlayer()
    {
        return PlayerMovement.instances[0].transform.position + PlayerMovement.instances[0].transform.forward * 2;
    }
    public Vector3 GetPositionNearMouse()
    {
        return FollowMouse.instance.transform.position;
    }
    public GameObject GetObjectNearPlayer(string name = "")
    {
        Vector3 point = GetPositionNearPlayer();
        SpawnedObjectList.Sort((GameObject a, GameObject b) => {
            float squaredRangeA = (a.transform.position - point).sqrMagnitude;
            float squaredRangeB = (b.transform.position - point).sqrMagnitude;
            return squaredRangeA.CompareTo(squaredRangeB);
        });
        return FilterViaName(name);
    }
    public GameObject GetObjectNearMouse(string name = "")
    {
        Vector3 point = GetPositionNearMouse();
        SpawnedObjectList.Sort((GameObject a, GameObject b) => {
            float squaredRangeA = (a.transform.position - point).sqrMagnitude;
            float squaredRangeB = (b.transform.position - point).sqrMagnitude;
            return squaredRangeA.CompareTo(squaredRangeB);
        });
        return FilterViaName(name);
    }
    public GameObject FilterViaName(string name)
    {
        if (name == "")
            return SpawnedObjectList[0];
        for (int i = 0; i < SpawnedObjectList.Count; i++)
        {
            if (SpawnedObjectList[i].name.ToLower().Contains(name))
            {
                return SpawnedObjectList[i];
            }
        }
        Debug.LogError("Object not found:" + name);
        return null;
    }
    /// <summary>
    /// Filter the id from the text
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string FilterName(string text)
    {
        for (int i = 0; i < PrefabList.Count; i++)
        {
            if (text.Contains(PrefabList[i].ID))
            {
                return PrefabList[i].ID;
            }
        }
        return "";
    }
    public GameObject GetPrefab(string text)
    {
        for (int i = 0; i < PrefabList.Count; i++)
        {
            if (text.Contains(PrefabList[i].ID))
            {
                return PrefabList[i].GO;
            }
        }
        Debug.LogError("Prefab not found");
        return null;
    }
    [System.Serializable]
    public class PrefabContainer
    {
        public string ID;
        public GameObject GO;
    }

}
