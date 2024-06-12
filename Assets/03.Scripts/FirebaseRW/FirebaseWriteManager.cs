using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;
using System;

public class FirebaseWriteManager : SingletonBehaviour<FirebaseWriteManager>
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    private FirebaseAuth _auth;
    private FirebaseUser _user;
    [Space]
    [Header("MBTI")]
    public TMP_InputField _mbtiInputField;

    internal void FetchCurrentUserMBTI(Action<string> updateMBTIInfo)
    {
        throw new NotImplementedException();
    }

    private DatabaseReference _databaseReference;

    public string CurrentUserMBTI { get; internal set; }

    void Start()
    {       
       StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();            
        }
        else
        {
            Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
        }
    }
    private void InitializeFirebase()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _user = _auth.CurrentUser;  // 현재 로그인된 사용자 가져오기
        if (_user != null)
        {
            Debug.Log("User is already logged in: " + _user.DisplayName);
        }
        else
        {
           Debug.LogError("No user is currently logged in.");
        }
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;        
    }
       

    public void SaveMBTI(string mbti)
    {        
        if (_user != null)
        {
            string userId = _user.UserId;
            string username = _user.DisplayName;
            _databaseReference.Child("USER").Child(userId).Child(username).Child("mbti").SetValueAsync(mbti).ContinueWith(task => { //Firebase에 user에 mbti 종속한걸 쓰기
                if (task.IsFaulted)
                {
                    Debug.LogError("Error writing MBTI to Firebase: " + task.Exception);
                }
                else
                {
                    Debug.Log("MBTI successfully saved.");
                }
            });
        }
        else
        {
            Debug.LogError("User not logged in. Please log in to save data.");
        }
    }    
}
