using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{

    private void Start()
    {

    }

    public void MoveScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
        Debug.Log("Scene Changed!");
    }
}
