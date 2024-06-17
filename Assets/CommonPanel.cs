using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// 텍스트 받아와서 적용시켜 주는 스크립트
public class CommonPanel : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI txtMain;

    private void OnEnable()
    {
        // 데이터를 올바르게 불러왔는지 확인
        if (FirebaseReadingManager.Instance.GetHobby1() == null) {
            Debug.Log($"CommonPanel.cs[{gameObject.name}] : 텍스트 가져오기 실패");
            return;
        }

        // 텍스트 적용
        txtMain.text = FirebaseReadingManager.Instance.GetHobby1();
        Debug.Log($"CommonPanel.cs[{gameObject.name}] : 텍스트 적용함 > '{txtMain.text}'");
    }
}
