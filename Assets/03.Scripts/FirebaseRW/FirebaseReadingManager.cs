// https://github.com/firebase/quickstart-unity.git 를 참조함
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class FirebaseReadingManager : Singleton<FirebaseReadingManager>
{
    [Header("Firebase")]
    // Firebase 종속성 상태 변수
    public DependencyStatus dependencyStatus;    
    [Space]
    [Header("MBTI Type")]
    public TMP_InputField MBTI_Field;
    
    // 데이터 저장 리스트    
    ArrayList leaderBoard = new ArrayList(); 
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


    private void Start()
    {   
        StartCoroutine(CheckAndFixDependenciesAsync(MBTI_Field.text));
    }

    private IEnumerator CheckAndFixDependenciesAsync(string mbti)
    {
        // Firebase 종속성 검사 시작
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

         // 비동기 작업이 완료될 때까지 대기
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        // 종속성 상태 결과를 받아옴
        dependencyStatus = dependencyTask.Result;

        // 종속성 상태에 따라 분기 처리
        if (dependencyStatus == DependencyStatus.Available) {        
            InitializeFirebaseDB(mbti);
        } else {        
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
    } 

    protected virtual void InitializeFirebaseDB(string mbti)
    {        
        FetchMBTIInfo(mbti);
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
}
