using UnityEngine;

public class UIManager : MonoBehaviour
{
    public LoginManager loginManager;
    public SignupManager signupManager;
    public LogoutManager logoutManager;

    void Start()
    {
        // 초기화 또는 다른 관리 작업을 여기서 수행할 수 있습니다.
        if (loginManager == null || signupManager == null || logoutManager == null)
        {
            Debug.LogError("One or more managers are not assigned in the Inspector.");
        }
    }
}
