using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthUIManager : MonoBehaviour
{
    public static AuthUIManager Instance;

    [SerializeField]
    private GameObject loginPanel;

    [SerializeField]
    private GameObject registrationPanel;

    [SerializeField]
    private GameObject gamePanel;

    [SerializeField]
    private GameObject emailVerificationPanel;

    [SerializeField]
    private Text emailVerificationText;

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void ClearUI()
    {
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
        gamePanel.SetActive(false);
    }
    public void OpenLoginPanel()
    {
        ClearUI();
        loginPanel.SetActive(true);
    }

    public void OpenRegistrationPanel()
    {
        ClearUI();
        registrationPanel.SetActive(true);
    }

    public void OpenGamePanel()
    {
        ClearUI();
        gamePanel.SetActive(true);
    }


    public void ShowVerificationResponse(bool isEmailSent, string emailId, string errorMessage)
    {
        ClearUI();
        emailVerificationPanel.SetActive(true);

        if (isEmailSent)
        {
            emailVerificationText.text = $"Please verify your email address \n Verification email has been sent to {emailId}";
        }
        else
        {
            emailVerificationText.text = $"Could't sent email : {errorMessage}";
        }
    }
}
