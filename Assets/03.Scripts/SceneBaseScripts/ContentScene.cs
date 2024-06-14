using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentScene : MonoBehaviour
{
    public TextMeshProUGUI mbtiText;
    private string userMbti;
    private void Awake()
    {
        userMbti= FirebaseReadingManager.Instance.CurrentUserMBTI;
        Debug.Log("UserMbti: "+ userMbti);
    }
    private void Start()
    {
        mbtiText.text = userMbti;
    }
}
