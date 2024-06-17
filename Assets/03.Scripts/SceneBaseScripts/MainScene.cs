using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;

public class MainScene : MonoBehaviour
{
    public TMP_Text user_MBTI;     // 사용자의 MBTI 
    public TMP_Text characteristic;     // MBTI 특성 
    public string userMBTI;

    public static MainScene Instance;

    private void Start()
    {
        FirebaseReadingManager.Instance.FetchCurrentUserMBTI();
        user_MBTI.text = "사용자의 MBTI 정보를 불러오는 중입니다.";
        characteristic.text = "MBTI 특성을 불러오는 중입니다.";
        StartCoroutine(WaitFetchTime());
    }

    private IEnumerator WaitFetchTime()
    {
        // fireReadingManager의 FirebaseReadingManager 컴포넌트가 준비될 때까지 대기
        yield return new WaitUntil(() =>
            FirebaseReadingManager.Instance._isRecall == true);
        // 조건이 충족되면 MBTI 특성 업데이트
        InfoMBTIData();
    }

    public void InfoMBTIData()
    {
        // FirebaseReadingManager에서 사용자 MBTI 데이터를 가져와 표시
        userMBTI = FirebaseReadingManager.Instance.CurrentUserMBTI;
        if (!string.IsNullOrEmpty(userMBTI))
        {
            string mbtiData = GetMBTIData(userMBTI);
            user_MBTI.text = "당신은 " + userMBTI + " 입니다";
            characteristic.text = mbtiData;
        }
        else
        {
            user_MBTI.text = "사용자의 MBTI 정보를 불러오는 중입니다.";
            characteristic.text = "MBTI 특성을 불러오는 중입니다.";
        }
    }

    private string GetMBTIData(string userMBTI)
    {
        Debug.Log("userMBTI: " + userMBTI);
        TextAsset jsonFile = Resources.Load<TextAsset>("MBTI_Dataset");
        if (jsonFile != null) Debug.Log("MBTI_Dataset 불러오기 완료");
        Dictionary<string, string> mbtiDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile.text);
        //Dictionary<string, string> mbtiDictionary = JsonUtility.FromJson<Dictionary<string, string>>(jsonFile.text);
        if (mbtiDictionary.ContainsKey(userMBTI))
        {
            return mbtiDictionary[userMBTI];
        }
        return userMBTI + "에 대한 MBTI 정보를 찾을 수 없습니다.";
    }
}

