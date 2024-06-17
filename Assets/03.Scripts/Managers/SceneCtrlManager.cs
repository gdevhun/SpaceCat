using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCtrlMananger : MonoBehaviour
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
    
    // 처음 씬에서 끌고오는 Soundmanager 싱글톤 버튼 캐싱을 위한 호출 함수   
    #region 
    public void PlaySceneSfx1() => SoundManager.Instance.PlaySceneSfx1();
    public void PlaySceneSfx2() => SoundManager.Instance.PlaySceneSfx2();
    public void PlayBtnSfx1() => SoundManager.Instance.PlayBtnSfx1();
    public void PlayBtnSfx2() => SoundManager.Instance.PlayBtnSfx2();
    public void PlayTypingSfx() => SoundManager.Instance.PlayTypingSfx();

    #endregion
  
}

