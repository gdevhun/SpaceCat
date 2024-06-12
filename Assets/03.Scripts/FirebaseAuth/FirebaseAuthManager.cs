// https://github.com/AakashGD890/FirebaseStarterProject
// https://firebase.google.com/docs/auth/unity/start?hl=ko)&_gl=1*2pq1it*_up*MQ..*_ga*NDUxNzQ2NTQ0LjE3MTQwMDYzODg.*_ga_CW55HF8NVT*MTcxNDAwNjM4OC4xLjAuMTcxNDAwNjM4OC4wLjAuMA.. (firebase 공식 docs)
// 를 참조함 

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;  // Necessary for using Task-based asynchronous programming
using TMPro;

public class FirebaseAuthManager : SingletonBehaviour<FirebaseAuthManager>
{
    // Firebase variables and references
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference databaseReference;

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
    public Toggle agreeTermsOfServiceToggle;

    [Space]
    [Header("FindIDPW")]
    public TMP_InputField emailResetField;  // 비밀번호 재설정을 위한 이메일 입력 필드

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

    // Initialize Firebase modelues
    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;
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
            UIManagerAuth.Instance.OpenLoginPanel();
        }
    }

    // 사용자의 이메일 인증 상태에 따른 자동 로그인 확인 및 처리 // Check and handle automatic login based on the user's email verification status
    private IEnumerator CheckForAutoLogin()
    {
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();
            yield return new WaitUntil(() => reloadUserTask.IsCompleted);

            if (user != null && user.IsEmailVerified)
            {
                AutoLogin();
            }
            else
            {
                UIManagerAuth.Instance.OpenLoginPanel();
            }
        }
    }

    // 자동 로그인 및 게임 패널로의 전환 처리 // Handle automatic login and transition to the game panel
    private void AutoLogin()
    {
        References.Instance.userName = user.DisplayName;
        UIManagerAuth.Instance.OpenGamePanel();
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
        if (!ValidateInput(email, "이메일을 입력해야 합니다.") ||
            !ValidateInput(password, "비밀번호를 입력해야 합니다."))
            yield break;

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
                UIManagerAuth.Instance.OpenGamePanel();
            }
            else
            {
                // 사용자에게 이메일 확인이 필요하다는 메시지 표시
                UIManagerAuth.Instance.ShowError("이메일 확인을 아직 안한 계정입니다. 메일을 확인해 주세요.");
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
        if (!ValidateInput(name, "이름을 입력해야 합니다.") ||
            !ValidateInput(email, "이메일을 입력해야 합니다.") ||
            !ValidateInput(password, "비밀번호를 입력해야 합니다.") ||
            !ValidateInput(confirmPassword, "비밀번호 확인을 입력해야 합니다."))
            yield break;

        if (password != confirmPassword)
        {
            UIManagerAuth.Instance.ShowError("비밀번호가 일치하지 않습니다.");
            yield break;
        }
        if (!agreeTermsOfServiceToggle.isOn)
        {
            UIManagerAuth.Instance.ShowError("서비스 약관에 동의해 주세요.");
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
                UIManagerAuth.Instance.ShowVerificationResponse(true, user.Email, null);
            }
        }
    }

    // 비밀번호 재설정 이메일을 보내는 메서드
    public void SendEmailForResetPassword()
    {
        StartCoroutine(SendEmailForResetPasswordAsync(emailResetField.text));
    }

    // 비밀번호 재설정 이메일을 비동기적으로 보내는 코루틴
    private IEnumerator SendEmailForResetPasswordAsync(string email)
    {
        if (!ValidateInput(email, "이메일을 입력해야 합니다."))
            yield break;

        var resetEmailTask = auth.SendPasswordResetEmailAsync(email);
        yield return new WaitUntil(() => resetEmailTask.IsCompleted);

        if (resetEmailTask.Exception != null)
        {
            HandleFirebaseError(resetEmailTask, "Reset Password");
        }
        else
        {
            Debug.Log("Password reset email sent successfully.");
        }
    }

    // Load the next scene
    public void OpenGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("NextScene");
    }

    // 입력 오류에 대한 처리
    private bool ValidateInput(string input, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            UIManagerAuth.Instance.ShowError(errorMessage);
            return false;
        }
        return true;
    }

    // 오류 처리를 중앙 집중화하여 Firebase 작업의 오류를 처리하는 메서드 // Method to handle errors from Firebase tasks, centralizing error handling
    private void HandleFirebaseError(Task task, string operation)
    {
        FirebaseException firebaseEx = task.Exception.Flatten().InnerExceptions[0] as FirebaseException;
        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

        string message = "Login Failed: " + errorCode.ToString();
        switch (errorCode)
        {
            case AuthError.InvalidEmail:
                message = "이메일 형식이 올바르지 않습니다. 올바른 이메일 주소를 입력해 주세요.";
                break;
            case AuthError.WrongPassword:
                message = "잘못된 비밀번호입니다. 비밀번호를 다시 확인해 주세요.";
                break;
            case AuthError.UserNotFound:
                message = "계정을 찾을 수 없습니다. 이메일 주소를 확인하거나 새 계정을 등록하세요.";
                break;
            case AuthError.UserDisabled:
                message = "사용자 계정이 비활성화되었습니다. 지원 팀에 문의하세요.";
                break;
            case AuthError.TooManyRequests:
                message = "요청이 너무 많습니다. 나중에 다시 시도하세요.";
                break;
            case AuthError.NetworkRequestFailed:
                message = "네트워크 오류가 발생했습니다. 인터넷 연결을 확인하세요.";
                break;
            case AuthError.OperationNotAllowed:
                message = "이메일 및 비밀번호 로그인이 활성화되지 않았습니다. 설정을 확인하세요.";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "이 이메일은 이미 사용 중입니다. 다른 이메일을 사용하세요.";
                break;
            case AuthError.WeakPassword:
                message = "비밀번호가 너무 약합니다. 더 강력한 비밀번호를 사용하세요.";
                break;
            case AuthError.RequiresRecentLogin:
                message = "최근 로그인 정보가 필요한 작업입니다. 다시 로그인해 주세요.";
                break;
            // 추가적인 오류 코드에 대한 케이스는 여기에 추가
            default:
                message = "알 수 없는 오류가 발생했습니다. 오류 코드: " + (firebaseEx != null ? errorCode.ToString() : "None");
                break;
        }

        UIManagerAuth.Instance.ShowError(message);
        Debug.LogError(message);
    }



}
