using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentScene : MonoBehaviour
{
    public TextMeshProUGUI mbtiText;
    private string userMbti;
    public TMP_InputField inputField;
    private string userInputData;
    public Button sendBtn;
    private void Awake()
    {
        userMbti= FirebaseReadingManager.Instance.CurrentUserMBTI;
        Debug.Log("UserMbti: "+ userMbti);
    }
    private void Start()
    {
        mbtiText.text = userMbti;
    }

    public void OnUserEditInput() => sendBtn.interactable = true;  //입력감지하면 버튼활성화 함수

    public void OnUserEndInput() //아무것도 입력하지 않았을 때 버튼예외처리를 위한 함수
    {
        userInputData = inputField.text;
        inputField.text = "";
        sendBtn.interactable = false;
    }
    public void SendOpinionBtn() =>  FirebaseWriteManager.Instance.SaveOpinion(userInputData); //firebase에 대화 등록 버튼

}
