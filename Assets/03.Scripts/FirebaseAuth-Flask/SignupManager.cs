using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField IDInput;
    public TMP_InputField passwordConfirmationInput;
    public Button signupButton;
    private string signupUrl = "http://127.0.0.1:5000/signup";

    void Start()
    {
        signupButton.onClick.AddListener(OnSignupButtonClick);
    }

    void OnSignupButtonClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string userID = IDInput.text;
        string passwordConfirmation = passwordConfirmationInput.text;

        // Validate inputs
        if (!ErrorHandlingManager.ValidateInput(email, "이메일을 입력해 주세요.") ||
            !ErrorHandlingManager.ValidateInput(userID, "사용자 ID를 입력해 주세요.") ||
            !ErrorHandlingManager.ValidateInput(password, "비밀번호를 입력해 주세요.") ||
            !ErrorHandlingManager.ValidateInput(passwordConfirmation, "비밀번호 확인을 입력해 주세요.") ||
            !ValidatePassword(password, passwordConfirmation))
        {
            return;
        }

        var json = new { email, password, userID };
        StartCoroutine(RequestHandler.PostRequest(signupUrl, json, OnSignupResponse));
    }

    void OnSignupResponse(string response)
    {
        Debug.Log("Signup Response: " + response);

        // Parse the response
        var responseObject = JsonUtility.FromJson<ResponseObject>(response);
        if (responseObject.status == "success")
        {
            // Handle successful signup (e.g., redirect to login screen or open main panel)
            Debug.Log("Signup successful.");
        }
        else
        {
            ErrorHandlingManager.HandleLoginError(responseObject.message);
        }
    }

    private bool ValidatePassword(string password, string passwordConfirmation)
    {
        if (password != passwordConfirmation)
        {
            UIManagerAuth.Instance.ShowError("비밀번호와 비밀번호 확인이 일치하지 않습니다.");
            return false;
        }
        return true;
    }
}
