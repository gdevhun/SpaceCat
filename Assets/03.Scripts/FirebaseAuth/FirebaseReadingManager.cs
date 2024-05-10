using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.Events;
using Firebase.Extensions;

public class FirebaseReadingManager : MonoBehaviour
{
    // 데이터 저장 리스트
    ArrayList leaderBoard = new ArrayList();
    public UnityEvent OnFirebaseInitialized = new UnityEvent();
    // Firebase 종속성 상태 변수
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;
    private string logText = "";
    //로그 저장 크기
    const int kMaxLogSize = 16382;
    //스크롤 변수
    private Vector2 scrollViewVector = Vector2.zero;


    private void Start()
    {     
        FirebaseApp.DefaultInstance.Options.DatabaseUrl =
                    new System.Uri("https://ossteamproject-a4ea0-default-rtdb.firebaseio.com/");
        StartCoroutine(StartFirebaseInitialization());
    }

    private IEnumerator StartFirebaseInitialization()
    {
        // Firebase 종속성 검사 시작
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

         // 비동기 작업이 완료될 때까지 대기
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        // 종속성 상태 결과를 받아옴
        dependencyStatus = dependencyTask.Result;

        // 종속성 상태에 따라 분기 처리
        if (dependencyStatus == DependencyStatus.Available) {        
            InitializeFirebaseDB();
        } else {        
            Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
    }
    } 

    protected virtual void InitializeFirebaseDB()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        StartListener();
        isFirebaseInitialized = true;
    }

    //순위 불러오기
    protected void StartListener()
    {
        FirebaseDatabase.DefaultInstance
          .GetReference("Leaders").OrderByChild("score")
          .ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
              if (e2.DatabaseError != null)
              {
                  Debug.LogError(e2.DatabaseError.Message);
                  return;
              }
              Debug.Log("Received values for Leaders.");
              string title = leaderBoard[0].ToString();
              leaderBoard.Clear();
              leaderBoard.Add(title);
              if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
              {
                  foreach (var childSnapshot in e2.Snapshot.Children)
                  {
                      if (childSnapshot.Child("score") == null
                        || childSnapshot.Child("score").Value == null)
                      {
                          Debug.LogError("Bad data in sample.");
                          break;
                      }
                      else
                      {
                          Debug.Log("Leaders entry : " +
                        childSnapshot.Child("email").Value.ToString() + " - " +
                        childSnapshot.Child("score").Value.ToString());
                          leaderBoard.Insert(1, childSnapshot.Child("score").Value.ToString()
                        + "  " + childSnapshot.Child("email").Value.ToString());
                      }
                  }
              }
          };
    }
    public void FetchMBTIDetail(string mbtiType) {
        if (string.IsNullOrEmpty(mbtiType)) {
            DebugLog("유효하지 않은 MBTI 입니다.");
            return;
        }
        DebugLog($"{mbtiType}에 대한 정보를 불러오는 중");

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("MBTI").Child(mbtiType);

        DebugLog("Fetching data...");
        reference.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null) {
                DebugLog(task.Exception.ToString());
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists && snapshot.HasChild("Detail")) {
                    string Detail = snapshot.Child("Detail").Value.ToString();
                    DebugLog($"Detail for {mbtiType}: {Detail}");
                } else {
                    DebugLog($"No detail found for MBTI type: {mbtiType}");
                }
            }
        });
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
}
