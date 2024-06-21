using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BackgroundTouchManager : MonoBehaviour
{
    public GameObject BackgroundBtn;
    public GameObject a1;
    public GameObject a2;

    public Animator anim;
    private bool _isClick;

    private void Start()
    {
        a1.SetActive(false);
        a2.SetActive(false);
    }

    public void ActiveArrowBtn()
    {
        _isClick = true;
        a1.SetActive(true);
        a2.SetActive(true);
        if (_isClick)
        {
            anim.SetBool("move", true);
            _isClick = false;
        }
    }

    public void StopAnim()
    {
        a1.SetActive(false);
        a2.SetActive(false);
        anim.SetBool("move", false);
    }
}
