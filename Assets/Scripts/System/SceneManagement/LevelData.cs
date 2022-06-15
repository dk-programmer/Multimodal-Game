using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "Data", menuName = "Data/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public List<ReferenceIdScene> SceneList;
    public bool SceneInLevel(Scene s)
    {
        for (int i = 0; i < SceneList.Count; i++)
        {
            if (s.path == SceneList[i].Scene.ScenePath)
            {
                return true;
            }
        }
        return false;
    }
    public void LoadLevel()
    {
        SceneLoadManager.Instance.ChangeLevel(this);
    }
    [System.Serializable]
    public class ReferenceIdScene
    {
        public SceneReference Scene;
    }
}
