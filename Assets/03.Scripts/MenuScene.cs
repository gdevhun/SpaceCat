using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    public Button checkBtn;
    public GameObject firebaseWriteManager;
    private string userMbti;

    //03.MenuScene -> MBTI 검사시작 또는 mbti 아는 경우 로직 처리 스크립트
    private List<string> validMbtiList = new List<string> {
        "istj", "isfp", "infj", "intj", "infp", "intp", "isfj", "istp",
        "estp", "esfp", "enfp", "entp", "estj", "esfj", "enfj", "entj"
    };
    //사용자 MBTI 입력 감지 호출 함수
    public void OnInputFieldEndEdit(string str)
    {
        //입력 MBTI가 유효한지 확인
        if (IsValidMbti(str))
        {
            checkBtn.GetComponent<Button>().interactable = true;
            //유효한 MBTI
            userMbti = str;
            firebaseWriteManager.GetComponent<FirebaseWriteManager>().SaveMBTI(userMbti);
        }
        else
        {
            //유효하지 않은 MBTI
            checkBtn.GetComponent<Button>().interactable = false;
        }
    }

    //MBTI가 유효한지 확인하는 함수
    private bool IsValidMbti(string mbti)
    {
        //입력 MBTI가 유효한 MBTI목록에 포함되는지 확인
        return validMbtiList.Contains(mbti.ToLower());
    }
    
}
