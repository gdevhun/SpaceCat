using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestScene : MonoBehaviour
{
    public SceneCtrlMananger sceneCtrlManager;
    public AudioSource gaugeSfx;
    public static TestScene Instance;

    public GameObject firstAnswerBtn;
    public GameObject secondAnswerBtn;

    //EvsI -> 1 5 9 13 17 21 25 29 33 37 
    //SvsN -> 2 6 10 14 18 22 26 30 34 38
    //TvsF -> 3 7 11 15 19 23 27 31 35 39
    //JvsP -> 4 8 12 16 20 24 28 32 36 40

    private bool _isTestOver;
    private int[] _result; //선택지결과를 저장할 배열 
    // E -> 0 I -> 1 S -> 2 N -> 3 T -> 4 F -> 5 J -> 6 P -> 7
    private int _curQuestionIndex; //현재질문 인덱스
    public string[] _questionStrings; //저장할 질문 스트링
    public string[] _answerString1; //저장할 답 스트링1
    public string[] _answerString2; //저장할 답 스트링2
    public TextMeshProUGUI questionText; //현재 보여주는 UI 질문텍스트
    public TextMeshProUGUI playerSelection1; //현재 보여주는 UI 선택지 1
    public TextMeshProUGUI playerSelection2; //현재 보여주는 UI 선택지 2
    public TextMeshProUGUI curQuestionNumber;
    public Image imageBar;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _result = new int[8];
        _questionStrings = new string[36]; 
        _answerString1 = new string[36]; 
        _answerString2 = new string[36];
        _curQuestionIndex = 0;
    }
   
    private void Start()
    {
        UpdateTextUI();
    }
    private void Update()
    {
        if (_isTestOver)
        {  
           //40개의 질문에 대한 대답 끝
           FirebaseWriteManager.Instance.SaveMBTI(ShowResult());
            sceneCtrlManager.MoveScene("04.MainScene"); 
           //MBTI 결과 씬 이동 결과지 보여주기.
        }
    }

    public string ShowResult() //받아온 데이터들을 연결
    {
        string firstMBTI;
        string secondMBTI;
        string thirdMBTI;
        string fourthMBTI;
        string finalResult;
        // E -> 0 I -> 1 S -> 2 N -> 3 T -> 4 F -> 5 J -> 6 F -> 7
        var e_num = _result[0];
        var i_num = _result[1];
        
        var s_num = _result[2];
        var n_num = _result[3];
        
        var t_num = _result[4];
        var f_num = _result[5];
        
        var j_num = _result[6];
        var p_num = _result[7];
        
        firstMBTI = e_num > i_num ? "E" : "I";  
        secondMBTI = s_num > n_num ? "S" : "N";
        thirdMBTI = t_num > f_num ? "T" : "F";
        fourthMBTI = j_num > p_num ? "J" : "P";
        finalResult = firstMBTI + secondMBTI + thirdMBTI + fourthMBTI;
        Debug.Log("finalResult: " + finalResult);
        return finalResult;
    }
    public void OnPlayerSelect(string buttonTag) //플레이어 선택버튼함수
    {
        
        switch (_curQuestionIndex % 4)
        {
            case 0: // 0으로 나눠떨어질 때의 동작 (P vs J)
                if (buttonTag == "FirstAnswer")
                {
                    _result[6]++;  //J 하나 증가
                }
                else if (buttonTag == "SecondAnswer")
                {
                    _result[7]++;  //P 하나 증가
                }
                break;
            case 1: // 1로 나눠떨어질 때의 동작  (E vs I)
                if (buttonTag == "FirstAnswer")
                {
                    Debug.Log("CompareTag FirstAnswer: E");
                    Debug.Log("_curQuestionIndex:" + _curQuestionIndex);
                    _result[0]++;  //E 하나 증가
                }
                else if (buttonTag == "SecondAnswer")
                {
                    Debug.Log("CompareTag SecondAnswer: I");
                    Debug.Log("_curQuestionIndex:" + _curQuestionIndex);
                    _result[1]++; //I 하나 증가
                }
                break;
            case 2: // 2로 나눠떨어질 때의 동작 (N vs S)
                if (buttonTag == "FirstAnswer")
                {
                    Debug.Log("CompareTag FirstAnswer: N");
                    _result[2]++; //S 하나 증가
                }
                else if (buttonTag == "SecondAnswer")
                {
                    Debug.Log("CompareTag SecondAnswer: S");
                    _result[3]++; //N 하나 증가
                }
                break;
            case 3: // 3으로 나눠떨어질 때의 동작 (F vs T)
                if (buttonTag == "FirstAnswer")
                {
                    _result[4]++; //T 하나 증가
                }
                else if (buttonTag == "SecondAnswer")
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
        Debug.Log("UpdateTextUI");
        _curQuestionIndex++;  //질문 인덱스 1증가 
        gaugeSfx.Play();
        if (_curQuestionIndex == 37)
        {   //만약 41번째라면 , 검사가 끝났다면
            _isTestOver = true;
            SoundManager.Instance.PlaySceneSfx1();
            return;
        }
            
        //각각 보이는 질문 답 UI 업데이트
        questionText.text = QALists.Instance._questionStrings[_curQuestionIndex-1];
        playerSelection1.text = QALists.Instance._answerString1[_curQuestionIndex-1];
        playerSelection2.text = QALists.Instance._answerString2[_curQuestionIndex-1];
        curQuestionNumber.text= $" {_curQuestionIndex} / 36";
        imageBar.fillAmount = (float)_curQuestionIndex / 36f;
    }
}

