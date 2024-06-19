using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Firebase.Auth;
using Google;
using Cysharp.Threading.Tasks;
using System.Text;

public class GoogleAuthentication : MonoBehaviour
{

    public string imageURL;
    public Text userNameTxt, userEmailTxt;
    public Image profilePic;
    public GameObject loginPanel, profilePanel;

    private GoogleSignInConfiguration configuration;

    private string webClientId = "1077253468932-j3e7ushv5trp8cgo0ss0dumh0l8j6518.apps.googleusercontent.com";
    private string webClientSecret = "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";
    private string redirectUri = "http://localhost:8080";
    private string tokenEndpoint = "https://oauth2.googleapis.com/token";
    private string authEndpoint = "https://accounts.google.com/o/oauth2/auth";
    private string responseType = "code";
    private string scope = "openid email profile";


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
        OpenGoogleLoginPage();

        //GoogleSignIn.Configuration = configuration;
        //GoogleSignIn.Configuration.UseGameSignIn = false;
        //GoogleSignIn.Configuration.RequestIdToken = true;
        //GoogleSignIn.Configuration.RequestEmail = true;

        //GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        //  OnAuthenticationFinished, TaskScheduler.Default);
    }

    // 구글 로그인 페이지 열기
    private void OpenGoogleLoginPage()
    {
        string authUrl = $"{authEndpoint}?client_id={webClientId}&redirect_uri={redirectUri}&response_type={responseType}&scope={scope}";
        Debug.Log($"Opening Google Login URL: {authUrl}");
        Application.OpenURL(authUrl);   // 브라우저에서 Google 로그인 페이지를 열기
    }

    // 애플리케이션 포커스 여부
    void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log($"Application has focus: {hasFocus}");
        if (!Application.absoluteURL.Contains("/auth?token=")) Debug.Log("Not Application.absoluteURL.Contains");
        if (hasFocus && Application.absoluteURL.Contains("/auth?token="))
        {
            string token = Application.absoluteURL.Substring(Application.absoluteURL.IndexOf("/auth?token=") + "/auth?token=".Length);
            if (!string.IsNullOrEmpty(token))
            {
                Debug.Log($"Received JWT: {token}");
                ExchangeJwtForFirebaseToken(token).Forget();    // JWT를 사용하여 Firebase 인증을 수행
            }
        }
    }


        //if (hasFocus && Application.absoluteURL.Contains("myapp://auth?code="))
        //{
        //    string code = Application.absoluteURL.Substring(Application.absoluteURL.IndexOf("myapp://auth?code=") + "myapp://auth?code=".Length);
        //    Debug.Log($"Received code: {code}");
        //    ExchangeCodeForToken(code).Forget();
        //}

        //if (hasFocus && Application.absoluteURL.StartsWith("unity:auth/"))
        //{
        //    string code = Application.absoluteURL.Substring("unity:auth/".Length);
        //    ExchangeCodeForToken(code).Forget();
        //}

    // 인증 코드를 사용하여 Firebase 인증을 수행
    private async UniTaskVoid ExchangeJwtForFirebaseToken(string jwtToken)
    {
        Debug.Log($"Exchanging JWT for Firebase token: {jwtToken}");

        // JWT를 파싱하여 ID 토큰 추출
        var parts = jwtToken.Split('.');
        var payload = parts[1];
        var jsonPayload = Encoding.UTF8.GetString(System.Convert.FromBase64String(payload));
        var idToken = JsonUtility.FromJson<Dictionary<string, string>>(jsonPayload)["idToken"];

        Debug.Log($"ID Token: {idToken}");

        // Firebase 인증 정보 설정
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        await FirebaseAuthManager.Instance.auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
        {
            if (authTask.IsCanceled || authTask.IsFaulted)
            {
                Debug.LogError("Firebase sign-in failed.");
                return;
            }

            FirebaseUser newUser = authTask.Result;

            // 사용자 정보를 Firebase Realtime Database에 저장
            User user = new User(newUser.DisplayName, newUser.Email, newUser.PhotoUrl != null ? newUser.PhotoUrl.ToString() : "");
            string json = JsonUtility.ToJson(user);

            FirebaseAuthManager.Instance.databaseReference.Child("users").Child(newUser.UserId).SetRawJsonValueAsync(json).ContinueWith(dbTask =>
            {
                if (dbTask.IsCompleted)
                {
                    Debug.LogError("Welcome: " + newUser.DisplayName + "!");

                    userNameTxt.text = newUser.DisplayName;
                    userEmailTxt.text = newUser.Email;
                    imageURL = newUser.PhotoUrl != null ? newUser.PhotoUrl.ToString() : "";

                    loginPanel.SetActive(false);
                    profilePanel.SetActive(true);
                    LoadProfilePic().Forget();
                }
                else
                {
                    Debug.LogError("Failed to save user data to Firebase.");
                }
            });
        });
    }

    //// Firebase 인증에 사용할 id_token 반환
    //private string ExtractTokenFromResponse(string response)
    //    {
    //        var json = JsonUtility.FromJson<Dictionary<string, string>>(response);
    //        return json["id_token"];
    //    }

    //    internal void OnAuthenticationFinished(string idToken)
    //    {
    //        // Firebase 인증 정보 설정
    //        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
    //        FirebaseAuthManager.Instance.auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
    //        {
    //            if (authTask.IsCanceled || authTask.IsFaulted)
    //            {
    //                Debug.LogError("Firebase sign-in failed.");
    //                return;
    //            }

    //            FirebaseUser newUser = authTask.Result;

    //            // 사용자 정보를 Firebase Realtime Database에 저장
    //            User user = new User(newUser.DisplayName, newUser.Email, newUser.PhotoUrl != null ? newUser.PhotoUrl.ToString() : "");
    //            string json = JsonUtility.ToJson(user);

    //            FirebaseAuthManager.Instance.databaseReference.Child("users").Child(newUser.UserId).SetRawJsonValueAsync(json).ContinueWith(dbTask =>
    //            {
    //                if (dbTask.IsCompleted)
    //                {
    //                    Debug.LogError("Welcome: " + newUser.DisplayName + "!");

    //                    userNameTxt.text = newUser.DisplayName;
    //                    userEmailTxt.text = newUser.Email;
    //                    imageURL = newUser.PhotoUrl != null ? newUser.PhotoUrl.ToString() : "";

    //                    loginPanel.SetActive(false);
    //                    profilePanel.SetActive(true);
    //                    LoadProfilePic().Forget();
    //                }
    //                else
    //                {
    //                    Debug.LogError("Failed to save user data to Firebase.");
    //                }
    //            });
    //        });
    //    }

    //internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    //{
    //    if (task.IsFaulted)
    //    {
    //        using (IEnumerator<System.Exception> enumerator =
    //                task.Exception.InnerExceptions.GetEnumerator())
    //        {
    //            if (enumerator.MoveNext())
    //            {
    //                GoogleSignIn.SignInException error =
    //                        (GoogleSignIn.SignInException)enumerator.Current;
    //                Debug.LogError("Got Error: " + error.Status + " " + error.Message);
    //            }
    //            else
    //            {
    //                Debug.LogError("Got Unexpected Exception?!?" + task.Exception);
    //            }
    //        }
    //    }
    //    else if (task.IsCanceled)
    //    {
    //        Debug.LogError("Canceled");
    //    }
    //    else
    //    {
    //        // Firebase 인증 정보 설정
    //        Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
    //        FirebaseAuthManager.Instance.auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
    //        {
    //            if (authTask.IsCanceled || authTask.IsFaulted)
    //            {
    //                Debug.LogError("Firebase sign-in failed.");
    //                return;
    //            }

    //            FirebaseUser newUser = authTask.Result;

    //            // 사용자 정보를 Firebase Realtime Database에 저장
    //            User user = new User(task.Result.DisplayName, task.Result.Email, task.Result.ImageUrl.ToString());
    //            string json = JsonUtility.ToJson(user);

    //            FirebaseAuthManager.Instance.databaseReference.Child("USER").Child(newUser.UserId).SetRawJsonValueAsync(json).ContinueWith(dbTask =>
    //            {
    //                if (dbTask.IsCompleted)
    //                {
    //                    Debug.LogError("Welcome: " + task.Result.DisplayName + "!");

    //                    userNameTxt.text = "" + task.Result.DisplayName;
    //                    userEmailTxt.text = "" + task.Result.Email;
    //                    imageURL = task.Result.ImageUrl.ToString();

    //                    loginPanel.SetActive(false);
    //                    profilePanel.SetActive(true);
    //                    StartCoroutine(LoadProfilePic());
    //                }
    //                else
    //                {
    //                    Debug.LogError("Failed to save user data to Firebase.");
    //                }
    //            });
    //        });

    //        loginPanel.SetActive(false);
    //        profilePanel.SetActive(true);
    //        StartCoroutine(LoadProfilePic());
    //    }
    //}


    private async UniTaskVoid LoadProfilePic()
    {
        if (!string.IsNullOrEmpty(imageURL))
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL))
            {
                await www.SendWebRequest().ToUniTask();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    var texture = DownloadHandlerTexture.GetContent(www);
                    profilePic.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                else
                {
                    Debug.LogError("Error loading profile pic: " + www.error);
                }
            }
        }
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
