using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestResult : MonoBehaviour
{
    //IvsE -> 1 5 9 13 17 21 25 29 33 37 
    //SvsN -> 2 6 10 14 18 22 26 30 34 38
    //TvsF -> 3 7 11 15 19 23 27 31 35 39
    //JvsP -> 4 8 12 16 20 24 28 32 36 40

    private bool _isTestOver;
    private int[] _result = new int[8]; //선택지결과를 저장할 배열 
    // I -> 0 E -> 1 S -> 2 N -> 3 T -> 4 F -> 5 P -> 6 J -> 7
    private int _curQuestionIndex; //현재질문 인덱스
    private string[] _questionStrings; //저장할 질문 스트링
    private string[] _answerString1; //저장할 답 스트링1
    private string[] _answerString2; //저장할 답 스트링2
    public TextMeshProUGUI questionText; //현재 보여주는 UI 질문텍스트
    public TextMeshProUGUI playerSelection1; //현재 보여주는 UI 선택지 1
    public TextMeshProUGUI playerSelection2; //현재 보여주는 UI 선택지 2

    private void Start()
    {
        _curQuestionIndex = 0;
        UpdateTextUI();
    }

    private void Update()
    {
        if (_isTestOver)
        {  
           //40개의 질문에 대한 대답 끝
           //결과지 보여주기.
        }
    }

    public void InitData() //받아온 데이터들을 연결
    {
        
    }
    public void OnPlayerSelect() //플레이어 선택버튼함수
    {
        
        switch (_curQuestionIndex % 4)
        {
            case 0: // 0으로 나눠떨어질 때의 동작 (P vs J)
                if (gameObject.CompareTag("FirstAnswer"))
                {
                    _result[6]++;  //P 하나 증가
                }
                else
                {
                    _result[7]++;  //J 하나 증가
                }
                break;
            case 1: // 1로 나눠떨어질 때의 동작  (I vs E)
                if (gameObject.CompareTag("FirstAnswer"))
                {
                    _result[0]++;  //I 하나 증가
                }
                else
                {
                    _result[1]++; //E 하나 증가
                }
                break;
            case 2: // 2로 나눠떨어질 때의 동작 (S vs N)
                if (gameObject.CompareTag("FirstAnswer"))
                {
                    _result[2]++; //S 하나 증가
                }
                else
                {
                    _result[3]++; //N 하나 증가
                }
                break;
            case 3: // 3으로 나눠떨어질 때의 동작 (T vs F)
                if (gameObject.CompareTag("FirstAnswer"))
                {
                    _result[4]++; //T 하나 증가
                }
                else
                {
                    _result[5]++; //F 하나 증가
                }
                break;
            default: // 예외 처리 또는 기본 동작
                break;
        }

        UpdateTextUI(); //다음 질문 UI 업데이트
    }
 
    private void UpdateTextUI()
    {
        _curQuestionIndex++;  //질문 인덱스 1증가 
        
        if (_curQuestionIndex == 41)
        {   //만약 41번째라면 , 검사가 끝났다면
            _isTestOver = true;
        }
        //각각 보이는 질문 답 UI 업데이트
        questionText.text = _questionStrings[_curQuestionIndex];
        playerSelection1.text = _answerString1[_curQuestionIndex];
        playerSelection2.text = _answerString2[_curQuestionIndex];
    }
}

