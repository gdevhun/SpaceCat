using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimCtrlManager : MonoBehaviour
{
    public enum CatAnimType
    {
        isRun,
        isJoy,
        isDead,
        isJump,
        isHit,
        isGravitation
    }
    private Animator catAnimator;
    void Start()
    {
        catAnimator = GetComponent<Animator>();
    }

    /*public void PlayCatAnimation(CatAnimType catAnimType)
    {
        
        catAnimator.SetTrigger(catAnimType.ToString());
    }*/
    
    public void PlayCatAnimRun()=>
        catAnimator.SetTrigger(CatAnimType.isRun.ToString());

    public void PlayCatAnimDead()=>
        catAnimator.SetTrigger(CatAnimType.isDead.ToString());
 
    public void PlayCatAnimJump()=>
        catAnimator.SetTrigger(CatAnimType.isJump.ToString());
   
    public void PlayCatAnimJoy()=>
        catAnimator.SetTrigger(CatAnimType.isJoy.ToString());
  
    public void PlayCatAnimHit()=>
        catAnimator.SetTrigger(CatAnimType.isHit.ToString());
 
    public void PlayCatAnimGravitation()=>
        catAnimator.SetTrigger(CatAnimType.isGravitation.ToString());
 
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
