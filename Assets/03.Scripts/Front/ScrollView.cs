using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class ScrollView : MonoBehaviour
{
    // 익명게시판 - 스크롤뷰 스크립트
    
    [SerializeField] private TMP_FontAsset mapleFontAsset; // 연결 폰트에셋
    private int totalOpinionNumber; // 익명 게시판 글 수
    private List<string> opinions = new List<string>();

    private void OnEnable()
    {
        FetchOpinions();
    }


    private void FetchOpinions()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("OpinionData");
        reference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.Log(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log("OpinionData Exists.");
                    opinions.Clear();
                    foreach (DataSnapshot mbtiSnapshot in snapshot.Children)
                    {
                        string userMbti = mbtiSnapshot.Key;
                        foreach (DataSnapshot userSnapshot in mbtiSnapshot.Children)
                        {
                            foreach (DataSnapshot opinionSnapshot in userSnapshot.Child("content").Children)
                            {
                                string opinionValue = userMbti + ": " + opinionSnapshot.Value.ToString();
                                opinions.Add(opinionValue);
                            }
                        }
                    }
                    totalOpinionNumber = opinions.Count;
                    CreateTexts();
                }
                else
                {
                    Debug.Log("No data found in OpinionData.");
                }
            }
        });
    }

    private void CreateTexts()
    {
        float startX = 0f;
        float startY = 200f;
        float spacing = 50f;

        foreach (string opinion in opinions)
        {
            CreateText(opinion, startX, startY);
            startY -= spacing;
        }
    }

    private void CreateText(string opinion, float startX, float startY)
    {
        float width = 800f;
        GameObject textObject = new GameObject("OpinionText");
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.font = mapleFontAsset;
        textComponent.text = opinion;
        textComponent.fontSize = 36;
        textComponent.alignment = TextAlignmentOptions.Left;
        textComponent.verticalAlignment = VerticalAlignmentOptions.Middle;
        textObject.transform.SetParent(this.transform, false);
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);    
        rectTransform.anchoredPosition = new Vector2(startX, startY);
    }
}