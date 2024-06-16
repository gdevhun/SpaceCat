using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    private string loginUrl = "http://127.0.0.1:5000/login";

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }

    void OnLoginButtonClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // Validate inputs
        if (!ErrorHandlingManager.ValidateInput(email, "이메일을 입력해 주세요.") ||
            !ErrorHandlingManager.ValidateInput(password, "비밀번호를 입력해 주세요."))
        {
            return;
        }

        var json = new { email, password };
        StartCoroutine(RequestHandler.PostRequest(loginUrl, json, OnLoginResponse));
    }

    void OnLoginResponse(string response)
    {
        Debug.Log("Login Response: " + response);

        // Parse the response
        var responseObject = JsonUtility.FromJson<ResponseObject>(response);
        if (responseObject.status == "success")
        {
            UIManagerAuth.Instance.OpenGamePanel();
        }
        else
        {
            ErrorHandlingManager.HandleLoginError(responseObject.message);
        }
    }
}
