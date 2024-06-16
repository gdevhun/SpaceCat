using UnityEngine;
using UnityEngine.UI;

public class LogoutManager : MonoBehaviour
{
    public Button logoutButton;
    private string logoutUrl = "http://127.0.0.1:5000/logout";

    void Start()
    {
        logoutButton.onClick.AddListener(OnLogoutButtonClick);
    }

    void OnLogoutButtonClick()
    {
        StartCoroutine(RequestHandler.PostRequest(logoutUrl, null, OnLogoutResponse));
    }

    void OnLogoutResponse(string response)
    {
        Debug.Log("Logout Response: " + response);
    }
}
