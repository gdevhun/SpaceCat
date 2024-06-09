using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MBTIInfo : MonoBehaviour
{
    public TMP_Text user_MBTI;     // 사용자의 MBTI 
    public TMP_Text characteristic;     // MBTI 특성 

    private void Start()
    {
        // FirebaseReadingManager에서 사용자 MBTI 데이터를 가져와 표시
        string userMBTI = FirebaseReadingManager.Instance.CurrentUserMBTI;
        if (!string.IsNullOrEmpty(userMBTI))
        {
            string mbtiData = GetMBTIData(userMBTI);
            characteristic.text = mbtiData;
            user_MBTI.text = "당신은 " + mbtiData + " 입니다";
        }
        else
        {
            user_MBTI.text = "사용자의 MBTI 정보를 불러오는 중입니다.";
            characteristic.text = "MBTI 특성을 불러오는 중입니다.";
        }
    }

    private string GetMBTIData(string userMBTI)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("MBTI_Dataset");
        Dictionary<string, string> mbtiDictionary = JsonUtility.FromJson<Dictionary<string, string>>(jsonFile.text);
        if (mbtiDictionary.ContainsKey(userMBTI))
        {
            return mbtiDictionary[userMBTI];
        }
        return userMBTI + "에 대한 MBTI 정보를 찾을 수 없습니다.";
    }
}

