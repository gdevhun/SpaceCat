using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Database;
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
        FetchOpinions().Forget();
    }

    private async UniTaskVoid FetchOpinions()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("OpinionData");

        Debug.Log("Fetching data...");
        DataSnapshot snapshot = await reference.GetValueAsync().AsUniTask();

        if (snapshot.Exists)
        {
            opinions.Clear();
            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                foreach (DataSnapshot opinionSnapshot in userSnapshot.Children)
                {
                    string opinionValue = opinionSnapshot.Value.ToString();
                    opinions.Add(opinionValue);
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

    private void CreateTexts()
    {
        foreach (string opinion in opinions)
        {
            CreateText(opinion);
        }
    }

    private void CreateText(string opinion)
    {
        GameObject textObject = new GameObject("OpinionText");
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.font = mapleFontAsset;
        textComponent.text = opinion;
        textComponent.fontSize = 36;
        textObject.transform.SetParent(this.transform, false);
    }
}