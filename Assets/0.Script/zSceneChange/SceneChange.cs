using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string nextSceneName;
    public void OnSceneChange()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
