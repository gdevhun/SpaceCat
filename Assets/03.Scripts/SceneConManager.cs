using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConMananger : Singleton<SceneConMananger>
{
    
    //01.IntroScene
    //02.LoginScene
    //03.MenuScene
    //03_01.MBTI_TestScene
    //04.MainScene
    //04_01.ChatScene
    //04_02.ContentScene
    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Scene Changed!");
    }
}

