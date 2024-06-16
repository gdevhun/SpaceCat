// https://github.com/firebase/quickstart-unity.git 를 참조함
using System;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;

public class FirebaseReadingManager : Singleton<FirebaseReadingManager>
{
    
     [Header("Firebase")]
    // Firebase 종속성 상태 변수
    public DependencyStatus dependencyStatus;
    //[Space]
    //[Header("MBTI Type")]
    //public TMP_InputField MBTI_Field;
    public bool _isRecall = true;
    private string _logText = "";
    //로그 저장 크기
    const int _kMaxLogSize = 16382;
    //스크롤 변수
    private Vector2 _scrollViewVector = Vector2.zero;
    //취미 변수
    private string _detail;
    private string _hobby1;
    private string _hobby2;
    private string _hobby3;
    // 사용자 MBTI 
    public string CurrentUserMBTI { get; internal set; } 
    //질문 변수
    private string _question;
    //현재 로그인된 사용자
    private FirebaseAuth _auth;
    private FirebaseUser _user;  

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
    public void FetchCurrentUserMBTI()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _user = _auth.CurrentUser;

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.
            GetReference("USER").Child(_user.UserId).Child(_user.DisplayName).Child("mbti");    //로그인시 user가 입력한 ID란의 값을 넣어야 불러옴
        reference.GetValueAsync().ContinueWithOnMainThread(async task => {
            if (task.Exception != null)
            {
                Debug.Log(task.Exception.ToString());
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    DebugLog("Found for current user.");
                    CurrentUserMBTI = snapshot.Value.ToString();
                    _isRecall = !_isRecall;
                    DebugLog("CurrentUserMBTI: " + CurrentUserMBTI);
                    await InitializeFirebaseDB(CurrentUserMBTI);
                }
                else
                {
                    DebugLog("No MBTI found for current user.");
                    await InitializeFirebaseDB("N/A");
                    CurrentUserMBTI = "N/A";
                }
            }
        });
    }

    public void FetchLatestUserData()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.
            GetReference("USER").Child(_user.UserId).Child(_user.DisplayName).Child("mbti");    //로그인시 user가 입력한 ID란의 값을 넣어야 불러옴
    }

    protected async UniTask InitializeFirebaseDB(string mbti)
    {
        Debug.Log("InitializeFirebase");
        await FetchAllQuestionsIInfo();              // 모든 MBTI 질문 리스트 불러오기
        Debug.Log("FetchMBTIInfo");
        //await FetchMBTIInfo(mbti);                    // 사용자에 대한 MBTI 정보 불러오기
        QALists.Instance.isInfoFetchCompleted = true;
        Debug.Log("TestScene.Instance.isInfoFetchCompleted = true;");
    }    

    // 출력 로그 관리
    public void DebugLog(string s)
    {
        Debug.Log(s);
        _logText += s + "\n";

        while (_logText.Length > _kMaxLogSize)
        {
            int index = _logText.IndexOf("\n");
            _logText = _logText.Substring(index + 1);
        }

        _scrollViewVector.y = int.MaxValue;
    }

    //MBTI에 따른 내용 불러오는 함수
    private async UniTask FetchMBTIInfo(string mbtiType) {
        if (string.IsNullOrEmpty(mbtiType)) {
            DebugLog("유효하지 않은 MBTI 입니다.");
            return;
        }
        


        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("MBTI").Child(mbtiType);

        DebugLog("Fetching data...");
        DataSnapshot snapshot = await reference.GetValueAsync().AsUniTask();

        if (snapshot.Exists)
        {
            if (snapshot.HasChild("Detail"))
            {
                _detail = snapshot.Child("Detail").Value.ToString();
                DebugLog($"Detail for {mbtiType}: {_detail}");
            }
            if (snapshot.HasChild("Hobby1"))
            {
                _hobby1 = snapshot.Child("Hobby1").Value.ToString();
                DebugLog($"Hobby1 for {mbtiType}: {_hobby1}");
            }
            if (snapshot.HasChild("Hobby2"))
            {
                _hobby2 = snapshot.Child("Hobby2").Value.ToString();
                DebugLog($"Hobby2 for {mbtiType}: {_hobby2}");
            }
            if (snapshot.HasChild("Hobby3"))
            {
                _hobby3 = snapshot.Child("Hobby3").Value.ToString();
                DebugLog($"Hobby3 for {mbtiType}: {_hobby3}");
            }
        }
        else
        {
            DebugLog($"No data found for MBTI type: {mbtiType}");
        }
    }
        

    // 모든 MBTI 질문 리스트 불러오기
    private async UniTask FetchAllQuestionsIInfo()
    {
        List<UniTask> fetchTasks = new List<UniTask>();
        for (int i = 1; i <= 36; i++)
        {
            string _question = i.ToString();
            fetchTasks.Add(FetchQuestionsInfo(_question)); // 리스트 i 번째의 데이터를 불러오기
        }
        await UniTask.WhenAll(fetchTasks);
    }

    // MBTI 번호에 따른 각 Q/A 불러오기
    private async UniTask FetchQuestionsInfo(string _question)
    {
        if (string.IsNullOrEmpty(_question))
        {
            DebugLog("유효하지 않은 문제입니다.");
            return;
        }
        DebugLog($"{_question}번 문제에 대한 정보를 불러오는 중");

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Questions").Child(_question);

        DebugLog("Fetching data...");
        DataSnapshot snapshot = await reference.GetValueAsync().AsUniTask();

        if (snapshot.Exists)
        {
            string questionData;
            int temp = int.Parse(_question);
            if (snapshot.HasChild("question"))
            {
                questionData = snapshot.Child("question").Value.ToString();
                DebugLog($"Question for {_question}: {questionData}");
                // Null 체크 추가
                if (QALists.Instance._questionStrings == null)
                {
                    DebugLog($"{_question}: TestScene.Instance._questionStrings is null");
                    return;
                }
                DebugLog($"{_question}: TestScene.Instance._questionStrings is not null");
                QALists.Instance._questionStrings[temp - 1] = questionData;
            }

            if (snapshot.HasChild("answer"))
            {
                // Null 체크 추가
                if (QALists.Instance._answerString1 == null || QALists.Instance._answerString2 == null)
                {
                    DebugLog("TestScene.Instance._answerString1 or _answerString2 is null");
                    return;
                }

                DataSnapshot answerSnapShot = snapshot.Child("answer");
                if (answerSnapShot.HasChild("a"))
                {
                    string answerA = answerSnapShot.Child("a").Value.ToString();
                    Debug.Log($"Answer A for (_list): {answerA}");
                    QALists.Instance._answerString1[temp - 1] = answerA;
                }
                if (answerSnapShot.HasChild("b"))
                {
                    string answerB = answerSnapShot.Child("b").Value.ToString();
                    Debug.Log($"Answer B for (_list): {answerB}");
                    QALists.Instance._answerString2[temp - 1] = answerB;
                }
            }
        }
        else
        {
            DebugLog($"No data found for question: {_question}");
        }
    }
    /*private async UniTask FetchOpinionInfo()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.
            GetReference("OpinionData");

        DebugLog("Fetching data...");
        DataSnapshot snapshot = await reference.GetValueAsync().AsUniTask();
        
        if (snapshot.Exists)
        {
            DebugLog($"Total opinions: {snapshot.ChildrenCount}");
            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                string userId = userSnapshot.Key;
                DebugLog($"User ID: {userId}");

                foreach (DataSnapshot opinionSnapshot in userSnapshot.Children)
                {
                    string opinionKey = opinionSnapshot.Key;
                    string opinionValue = opinionSnapshot.Value.ToString();
                    DebugLog($"Opinion ID: {opinionKey}, Opinion: {opinionValue}");
                }
            }
        }
        else
        {
            DebugLog("No data found in OpinionData.");
        }
    }*/
    

    public string GetDetail()
    {
        return _detail;
    }
    public string GetHobby1()
    {
        return _hobby1;
    }

    public string GetHobby2()
    {
        return _hobby2;
    }

    public string GetHobby3()
    {
        return _hobby3;
    }   

    public string GetQuestions()
    {
        return _question;
    }
}
