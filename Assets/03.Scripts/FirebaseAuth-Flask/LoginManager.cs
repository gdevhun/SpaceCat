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
        var json = new { email, password };
        StartCoroutine(RequestHandler.PostRequest(loginUrl, json, OnLoginResponse));
    }

    void OnLoginResponse(string response)
    {
        Debug.Log("Login Response: " + response);
    }
}
