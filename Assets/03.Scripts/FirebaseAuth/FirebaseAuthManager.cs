// https://github.com/AakashGD890/FirebaseStarterProject 참조

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;  // Necessary for using Task-based asynchronous programming
using TMPro;

public class FirebaseAuthManager : MonoBehaviour
{
    // Firebase variables and references
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    // UI elements for login
    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    // UI elements for registration
    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    private void Start()
    {
        // 스크립트 시작 시 Firebase 종속성 확인 // Begin checking for Firebase dependencies as soon as the script starts
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    // 모든 Firebase 종속성 확인 및 사용가능 확인 // Coroutine to check and ensure all Firebase dependencies are available
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

    // Initialize Firebase services
    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged -= AuthStateChanged;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null); // 인증 상태 초기 확인 트리거 // Trigger initial check of auth state
    }

    // Firebase 인증 상태 변경 핸들러 // Handler for Firebase authentication state changes
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        FirebaseUser oldUser = user;
        user = auth.CurrentUser;

        if (oldUser != user && user != null)
        {
            Debug.Log("Signed in: " + user.UserId);
            StartCoroutine(CheckForAutoLogin());
        }
        else if (user == null)
        {
            Debug.Log("Signed out");
            AuthUIManager.Instance.OpenLoginPanel();
        }
    }

    // 사용자의 이메일 인증 상태에 따른 자동 로그인 확인 및 처리 // Check and handle automatic login based on the user's email verification status
    private IEnumerator CheckForAutoLogin()
    {
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();
            yield return new WaitUntil(() => reloadUserTask.IsCompleted);

            if (user.IsEmailVerified)
            {
                AutoLogin();
            }
            else
            {
                AuthUIManager.Instance.OpenLoginPanel();
            }
        }
    }

    // 자동 로그인 및 게임 패널로의 전환 처리 // Handle automatic login and transition to the game panel
    private void AutoLogin()
    {
        References.Instance.userName = user.DisplayName;
        AuthUIManager.Instance.OpenGamePanel();
    }

    // 현재 사용자를 '로그아웃'하는 공개 메서드 // Public method to log out the current user
    public void Logout()
    {
        if (auth != null && user != null)
        {
            auth.SignOut();
        }
    }

    // 로그인 프로세스를 시작하는 메서드 // Public method to initiate the login process
    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    // Coroutine to handle user login
    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            HandleFirebaseError(loginTask, "Login");
        }
        else
        {
            user = loginTask.Result.User;
            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);

            if (user.IsEmailVerified)
            {
                References.Instance.userName = user.DisplayName;
                AuthUIManager.Instance.OpenGamePanel();
            }
            else
            {
                SendEmailForVerification();
            }
        }
    }


    // 가입 프로세스를 시작하는 메서드 // Public method to initiate the registration process
    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    // Coroutine to handle user registration
    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "" || email == "" || password == "" || confirmPassword == "")
        {
            Debug.LogError("All fields must be filled.");
            yield break;
        }

        if (password != confirmPassword)
        {
            Debug.LogError("Passwords do not match.");
            yield break;
        }

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            HandleFirebaseError(registerTask, "Registration");
        }
        else
        {
            user = registerTask.Result.User;
            UserProfile userProfile = new UserProfile { DisplayName = name };
            var updateProfileTask = user.UpdateUserProfileAsync(userProfile);
            yield return new WaitUntil(() => updateProfileTask.IsCompleted);

            if (updateProfileTask.Exception != null)
            {
                user.DeleteAsync(); // Roll back user creation
                HandleFirebaseError(updateProfileTask, "Update Profile");
            }
            else
            {
                Debug.Log("Registration Successful. Welcome " + user.DisplayName);
                SendEmailForVerification();
            }
        }
    }

    // 사용자에게 이메일 인증 전송 메서드 // Method to send an email verification to the user
    public void SendEmailForVerification()
    {
        StartCoroutine(SendEmailForVerificationAsync());
    }

    // Coroutine to handle the sending of the email verification
    private IEnumerator SendEmailForVerificationAsync()
    {
        if (user != null)
        {
            var sendEmailTask = user.SendEmailVerificationAsync();
            yield return new WaitUntil(() => sendEmailTask.IsCompleted);

            if (sendEmailTask.Exception != null)
            {
                HandleFirebaseError(sendEmailTask, "Send Email Verification");
            }
            else
            {
                Debug.Log("Email verification sent successfully to " + user.Email);
                AuthUIManager.Instance.ShowVerificationResponse(true, user.Email, null);
            }
        }
    }

    // Load the next scene
    public void OpenGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("NextScene");
    }

    // 오류 처리를 중앙 집중화하여 Firebase 작업의 오류를 처리하는 메서드 // Method to handle errors from Firebase tasks, centralizing error handling
    private void HandleFirebaseError(Task task, string context)
    {
        FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
        if (firebaseException != null)
        {
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;
            string errorMessage = $"{context} Error: {errorCode.ToString()}";
            Debug.LogError(errorMessage);
            // AuthUIManager.Instance.ShowMessage(errorMessage); // TODO // message box 구현 
        }
        else
        {
            Debug.LogError($"{context} failed with unknown error.");
            // AuthUIManager.Instance.ShowMessage($"{context} failed with unknown error.");
        }
    }

}
