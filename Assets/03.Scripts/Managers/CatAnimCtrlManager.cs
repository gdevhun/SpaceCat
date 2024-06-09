using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimCtrlManager : Singleton<CatAnimCtrlManager>
{
    public enum CatAnimType
    {
        CatRun,
        CatJoy,
        CatDead,
        CatJump,
        CatHit,
        CatGravitation
    }
    private Animator catAnimator;
    void Start()
    {
        catAnimator = GetComponent<Animator>();
    }

    public void PlayCatAnimation(CatAnimType catAnimType)
    {
        
        catAnimator.SetTrigger(catAnimType.ToString());
    }
    /*
     //에니메이션 작동 테스트 코드
     
     private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            catAnimator.SetTrigger("isRun");
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            catAnimator.SetTrigger("isJoy");
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            catAnimator.SetTrigger("isDead");
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            catAnimator.SetTrigger("isGraviation");
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            catAnimator.SetTrigger("isHit");
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            catAnimator.SetTrigger("isJump");
        }
    }*/
}
