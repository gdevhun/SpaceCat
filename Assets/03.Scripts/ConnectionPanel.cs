using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[Serializable]
public class Mbti
{
    public enum Type
    {
        
        ISTJ, ISFP, INFJ, INTJ, INFP, INTP, ISFJ, ISTP,
        ESTP, ESFP, ENFP, ENTP, ESTJ, ESFJ, ENFJ, ENTJ
    }
    public Type mbtiType;
    public string goodMbtiString;
    public string badMbtiString;
    public Sprite mbtiSprite;
}
public class ConnectionPanel : MonoBehaviour
{
    [SerializeField]
    private List<Mbti> mbtis;

    public Image mbtiImage;
    public TextMeshProUGUI goodTxt;
    public TextMeshProUGUI badTxt;
    
    private string userMbti;

    private void OnEnable()
    {
        userMbti = FirebaseReadingManager.Instance.CurrentUserMBTI;

        // Convert userMbti string to Mbti.Type enum
        if (Enum.TryParse(userMbti, out Mbti.Type userMbtiType))
        {
            // Find Mbti object by mbtiType
            Mbti userMbtiObject = mbtis.Find(mbti => mbti.mbtiType == userMbtiType);

            if (userMbtiObject != null)
            {
                // userMbti에 해당하는 mbti 객체를 찾았을 때 수행할 작업
                Debug.Log("User MBTI found: " + userMbtiObject.mbtiType);
                mbtiImage.sprite = userMbtiObject.mbtiSprite;
                goodTxt.text = userMbtiObject.goodMbtiString;
                badTxt.text = userMbtiObject.badMbtiString;
            }
            else
            {
                Debug.LogError("User MBTI not found: " + userMbti);
            }
        }
        else
        {
            Debug.LogError("Invalid MBTI type: " + userMbti);
        }
    
    }
    
}
