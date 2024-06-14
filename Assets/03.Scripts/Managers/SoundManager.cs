using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private bool isPlaying;
    public AudioSource audioSource;
    
    public AudioClip sceneMoveSfx;
    public AudioClip sceneMoveSfx2;

    public AudioClip btnSfx1;
    public AudioClip btnSfx2;
    public AudioClip typingSfx;
  
    public void PlaySceneSfx1()
    {
        audioSource.clip = sceneMoveSfx;
        audioSource.PlayOneShot(sceneMoveSfx);
    }

    public void PlaySceneSfx2()
    {
        audioSource.clip = sceneMoveSfx2;
        audioSource.PlayOneShot(sceneMoveSfx2);
    }

    public void PlayTypingSfx()
    {
        if (isPlaying) return;
        
        isPlaying = true;
        audioSource.PlayOneShot(typingSfx);
        WaitTypingTime().Forget();
    }
    public void PlayBtnSfx1()
    {
        audioSource.clip = btnSfx1;
        audioSource.PlayOneShot(btnSfx1);
    } 
    public void PlayBtnSfx2()
    {
        audioSource.clip = btnSfx1;
        audioSource.PlayOneShot(btnSfx2);
    }

    private async UniTaskVoid WaitTypingTime()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        isPlaying = false;
    }
}
