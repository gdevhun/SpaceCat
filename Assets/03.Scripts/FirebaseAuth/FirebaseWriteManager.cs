using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;

public class FirebaseWriteManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;
    [Space]
    [Header("MBTI")]
    public TMP_InputField mbtiInputField;
    private DatabaseReference databaseReference;
    // Start is called before the first frame update
    void Start()
    {       
       StartCoroutine(CheckAndFixDependenciesAsync());
       mbtiInputField.onValueChanged.AddListener(delegate { SaveMBTI(mbtiInputField.text); });
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
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;  // 현재 로그인된 사용자 가져오기
        if (user != null)
        {
            Debug.Log("User is already logged in: " + user.UserId);
        }
        else
        {
            Debug.LogError("No user is currently logged in.");
        }
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;        
    }

    public void SaveMBTI(string mbti)
    {
        if (user != null)
        {
            string userId = user.UserId;
            databaseReference.Child("USER").Child(userId).Child("mbti").SetValueAsync(mbti).ContinueWith(task => {
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
