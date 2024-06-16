using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
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
        var json = new { email, password };
        StartCoroutine(RequestHandler.PostRequest(signupUrl, json, OnSignupResponse));
    }

    void OnSignupResponse(string response)
    {
        Debug.Log("Signup Response: " + response);
    }
}
