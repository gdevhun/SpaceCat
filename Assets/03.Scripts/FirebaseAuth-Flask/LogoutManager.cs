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

        // Parse the response
        var responseObject = JsonUtility.FromJson<ResponseObject>(response);
        if (responseObject.status == "success")
        {
            // Handle successful logout (e.g., redirect to login screen)
            Debug.Log("Logout successful.");
        }
        else
        {
            ErrorHandlingManager.HandleLoginError(responseObject.message);
        }
    }
}
