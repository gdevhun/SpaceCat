using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Google;
using TMPro;

public class GoogleAuthentication : MonoBehaviour
{

    public string imageURL;

    public Text userNameTxt, userEmailTxt;

    public Image profilePic;

    public GameObject loginPanel, profilePanel;

    public string webClientId = "1077253468932-j3e7ushv5trp8cgo0ss0dumh0l8j6518.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;



    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake()
    {
        // Google Sign-In 초기 설정
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true,
            RequestEmail = true
        };
    }


    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished, TaskScheduler.Default);
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.LogError("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Canceled");
        }
        else
        {
            // Firebase 인증 정보 설정
            Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            FirebaseAuthManager.Instance.auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
            {
                if (authTask.IsCanceled || authTask.IsFaulted)
                {
                    Debug.LogError("Firebase sign-in failed.");
                    return;
                }

                FirebaseUser newUser = authTask.Result;

                // 사용자 정보를 Firebase Realtime Database에 저장
                User user = new User(task.Result.DisplayName, task.Result.Email, task.Result.ImageUrl.ToString());
                string json = JsonUtility.ToJson(user);

                FirebaseAuthManager.Instance.databaseReference.Child("USER").Child(newUser.UserId).SetRawJsonValueAsync(json).ContinueWith(dbTask =>
                {
                    if (dbTask.IsCompleted)
                    {
                        Debug.LogError("Welcome: " + task.Result.DisplayName + "!");

                        userNameTxt.text = "" + task.Result.DisplayName;
                        userEmailTxt.text = "" + task.Result.Email;
                        imageURL = task.Result.ImageUrl.ToString();

                        loginPanel.SetActive(false);
                        profilePanel.SetActive(true);
                        StartCoroutine(LoadProfilePic());
                    }
                    else
                    {
                        Debug.LogError("Failed to save user data to Firebase.");
                    }
                });
            });

            loginPanel.SetActive(false);
            profilePanel.SetActive(true);
            StartCoroutine(LoadProfilePic());
        }
    }

    IEnumerator LoadProfilePic()
    {
        WWW www = new WWW(imageURL);
        yield return www;

        profilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }

    public void OnSignOut()
    {
        userNameTxt.text = "";
        userEmailTxt.text = "";

        imageURL = "";
        loginPanel.SetActive(true);
        profilePanel.SetActive(false);

        Debug.LogError("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    [System.Serializable]
    public class User
    {
        public string username;
        public string email;
        public string profileImageUrl;

        public User(string username, string email, string profileImageUrl)
        {
            this.username = username;
            this.email = email;
            this.profileImageUrl = profileImageUrl;
        }
    }
}
