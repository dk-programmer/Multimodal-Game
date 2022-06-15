using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Events;
public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;
    public Actions.OnAction OnStartLoadLevel,OnEndLoadLevel;
    [SerializeField] private LevelData CurrentLevel;
    private Coroutine LevelChangeRoutine;
    [SerializeField] private float Progress;
    [SerializeField] private LevelData data;
    public void Awake()
    {
        Instance = this;
    }
    public bool IsSceneActive(string path)
    {
        Scene scene = SceneManager.GetSceneByPath(path);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i) == scene)
            {
                return true;
            }
        }
        return false;
    }
    [Button]
    public void ChangeLevel(LevelData level)
    {
        if (LevelChangeRoutine != null) return;
            LevelChangeRoutine = StartCoroutine(StartLevelChange(level));

    }
    List<AsyncOperation> UnloadUnusedScenes(LevelData level)
    {
        List<AsyncOperation> async_list = new List<AsyncOperation>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (!level.SceneInLevel(SceneManager.GetSceneAt(i)))
            {
                async_list.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i)));
            }
        }
        return async_list;
    }
    List<AsyncOperation> LoadLevel(LevelData level)
    {
        List<AsyncOperation> async_list = new List<AsyncOperation>();
        for (int i = 0; i < level.SceneList.Count; i++)
        {
            if (!IsSceneActive(level.SceneList[i].Scene.ScenePath))
            {
                async_list.Add(SceneManager.LoadSceneAsync(level.SceneList[i].Scene, LoadSceneMode.Additive));
            }
        }
        return async_list;
    }
    IEnumerator StartLevelChange(LevelData level)
    {
        OnStartLoadLevel?.Invoke();
        yield return new WaitForSeconds(1);
        List<AsyncOperation> async_list = new List<AsyncOperation>();
        async_list.AddRange(UnloadUnusedScenes(level));
        async_list.AddRange(LoadLevel(level));
        yield return new WaitForEndOfFrame();
        while (!AsyncListComplete(async_list))
        {
            Progress = GetAsyncListProgress(async_list);
            yield return new WaitForEndOfFrame();
        }
        CurrentLevel = level;
        yield return new WaitForSeconds(0.1f);
        OnEndLoadLevel?.Invoke();
        LevelChangeRoutine = null;
    }
    public float GetAsyncListProgress(List<AsyncOperation> async_list)
    {
        float value = 0;
        for (int i = 0; i < async_list.Count; i++)
        {
            value = async_list[i].progress;
        }
        return value / async_list.Count;
    }
    public bool AsyncListComplete(List<AsyncOperation> async_list)
    {
        for (int i = 0; i < async_list.Count; i++)
        {
            if (!async_list[i].isDone) return false;
        }
        return true;
    }
}
