// https://github.com/firebase/quickstart-unity.git 를 참조함
using System;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class FirebaseReadingManager : MonoBehaviour
{
    [Header("Firebase")]
    // Firebase 종속성 상태 변수
    public DependencyStatus dependencyStatus;    
    //[Space]
    //[Header("MBTI Type")]
    //public TMP_InputField MBTI_Field;
          
    protected bool isFirebaseInitialized = false;
    private string logText = "";
    //로그 저장 크기
    const int kMaxLogSize = 16382;
    //스크롤 변수
    private Vector2 scrollViewVector = Vector2.zero;
    //취미 변수
    private string Detail;
    private string hobby1;
    private string hobby2;
    private string hobby3;
    //질문 변수
    private string question;
    //현재 로그인된 사용자
    private FirebaseAuth auth;
    private FirebaseUser user;  

    private void Start()
    {   
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        // Firebase 종속성 검사 시작
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

         // 비동기 작업이 완료될 때까지 대기
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        // 종속성 상태 결과를 받아옴
        dependencyStatus = dependencyTask.Result;

        // 종속성 상태에 따라 분기 처리
        if (dependencyStatus == DependencyStatus.Available) {
            FetchCurrentUserMBTI();
        } else {        
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
    }

    // 현재 사용자 정보 가져오는 메소드
    private void FetchCurrentUserMBTI()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("USER").Child(user.DisplayName).Child("mbti");//로그인시 user가 입력한 ID란의 값을 넣어야 불러옴
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
                    string currentUserMBTI = snapshot.Value.ToString();
                    string Questions = snapshot.Value.ToString();
                    InitializeFirebaseDB(currentUserMBTI, Questions);
                }
                else
                {
                    DebugLog("No MBTI found for current user.");
                }
            }
        });
    }

    protected virtual void InitializeFirebaseDB(string mbti, string _question)
    {
        FetchAllQuestionsIInfo();              // 모든 MBTI 질문 리스트 불러오기
        FetchQuestionsInfo(_question);    // MBTI Q/A 불러오기
        FetchMBTIInfo(mbti);                    // 사용자에 대한 MBTI 정보 불러오기
        isFirebaseInitialized = true;
    }    

    // 출력 로그 관리
    public void DebugLog(string s)
    {
        Debug.Log(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }

        scrollViewVector.y = int.MaxValue;
    }

    //MBTI에 따른 내용 불러오는 함수
    public void FetchMBTIInfo(string mbtiType) {
        if (string.IsNullOrEmpty(mbtiType)) {
            DebugLog("유효하지 않은 MBTI 입니다.");
            return;
        }
        DebugLog($"{mbtiType}에 대한 정보를 불러오는 중");

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("MBTI").Child(mbtiType);

        DebugLog("Fetching data...");
        reference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                Debug.Log(task.Exception.ToString());
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists) {
                    if (snapshot.HasChild("Detail")) {
                        Detail = snapshot.Child("Detail").Value.ToString();
                        DebugLog($"Detail for {mbtiType}: {Detail}");
                    } 
                    if (snapshot.HasChild("Hobby1")) {
                        hobby1 = snapshot.Child("Hobby1").Value.ToString();
                        DebugLog($"Hobby1 for {mbtiType}: {hobby1}");
                    }
                    if (snapshot.HasChild("Hobby2")) {
                        hobby2 = snapshot.Child("Hobby2").Value.ToString();
                        DebugLog($"Hobby2 for {mbtiType}: {hobby2}");
                    }
                    if (snapshot.HasChild("Hobby3")) {
                        hobby3 = snapshot.Child("Hobby3").Value.ToString();
                        DebugLog($"Hobby3 for {mbtiType}: {hobby3}");
                    }
                } else {
                    DebugLog($"No data found for MBTI type: {mbtiType}");
                }
            }
        });
    }

    // 모든 MBTI 질문 리스트 불러오기
    public void FetchAllQuestionsIInfo()
    {
        for(int i = 1; i <= 40; i++)
        {
            string _question = i.ToString();
            FetchQuestionsInfo(_question); // 리스트 i 번째의 데이터를 불러오기
        }
    }

    // MBTI 번호에 따른 각 Q/A 불러오기
    public void FetchQuestionsInfo(string _question)
    {
        if (string.IsNullOrEmpty(_question))
        {
            DebugLog("유효하지 않은 문제입니다.");
            return;
        }
        DebugLog($"{_question}번 문제에 대한 정보를 불러오는 중");

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Questions").Child(_question);

        DebugLog("Fetching data...");
        reference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // AggregateException을 평탄화하여 각각의 예외를 처리
                foreach (Exception ex in task.Exception.Flatten().InnerExceptions)
                {
                    Debug.Log($"Exception caught: {ex.Message}");
                    Debug.Log(task.Exception.ToString());
                }
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // "question" 데이터 읽기
                    if (snapshot.HasChild("question"))
                    {
                        question = snapshot.Child("question").Value.ToString();
                        DebugLog($"Question for {_question}: {question}");
                    }

                    // "answer" 데이터 읽기
                    if (snapshot.HasChild("answer"))
                    {
                        DataSnapshot answerSnapShot = snapshot.Child("answer");
                        if (answerSnapShot.HasChild("a"))
                        {
                            string answerA = answerSnapShot.Child("a").Value.ToString();
                            Debug.Log($"Answer A for (_list): {answerA}");
                        }
                        if (answerSnapShot.HasChild("b"))
                        {
                            string answerB= answerSnapShot.Child("b").Value.ToString();
                            Debug.Log($"Answer A for (_list): {answerB}");
                        }
                        if (answerSnapShot.HasChild("re1"))
                        {
                            string typeRe1 = answerSnapShot.Child("re1").Value.ToString();
                            Debug.Log($"Answer re1 for (_list): {typeRe1}");
                        }
                        if (answerSnapShot.HasChild("re2"))
                        {
                            string typeRe2 = answerSnapShot.Child("re2").Value.ToString();
                            Debug.Log($"Answer re2 for (_list): {typeRe2}");
                        }
                    }
                }
                else
                {
                    DebugLog($"No data found for question: {_question}");
                }
            }
        });
    }

    public string GetDetail()
    {
        return Detail;
    }
    public string GetHobby1()
    {
        return hobby1;
    }

    public string GetHobby2()
    {
        return hobby2;
    }

    public string GetHobby3()
    {
        return hobby3;
    }   

    public string GetQuestions()
    {
        return question;
    }
}
