using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Vector2 nextPos;
    public string nextSceneName;

    public void SceneChange(Transform p)
    {
        SceneManager.LoadScene(nextSceneName);
        p.position = nextPos;
    }
}
