using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerReference : MonoBehaviour
{
    public void LoadLevel(LevelData level)
    {
        SceneLoadManager.Instance.ChangeLevel(level);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
