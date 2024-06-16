using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManagerAuth : Singleton<UIManagerAuth>
{
    [Header("Panel")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registrationPanel;
    [SerializeField] private GameObject findIDPWPanel;
    [SerializeField] private GameObject termOfServicePanel;
    [SerializeField] private GameObject profilePanel;

    [Space]
    [Header("Email")]
    [SerializeField] private TMP_Text emailVerificationText;
    [SerializeField] private GameObject emailVerificationPanel;

    [Space]
    [Header("Error")]
    [SerializeField] private GameObject errorPanel; // 에러 메시지를 표시할 패널
    [SerializeField] private TMP_Text errorMessageText; // 에러 메시지를 표시할 TextMeshPro 텍스트 컴포넌트

    // Dictionary to hold the InputFields for each panel
    private Dictionary<GameObject, List<TMP_InputField>> panelInputFields;
    private List<TMP_InputField> currentInputFields;

    private void Awake()
    {
        InitializeInputFields();
    }

    private void InitializeInputFields()
    {
        panelInputFields = new Dictionary<GameObject, List<TMP_InputField>>()
        {
            { loginPanel, new List<TMP_InputField>(loginPanel.GetComponentsInChildren<TMP_InputField>(true)) },
            { registrationPanel, new List<TMP_InputField>(registrationPanel.GetComponentsInChildren<TMP_InputField>(true)) }
        };
        currentInputFields = new List<TMP_InputField>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && currentInputFields.Count > 0)
        {
            MoveToNextInputField();
        }
    }

    private void MoveToNextInputField()
    {
        EventSystem system = EventSystem.current;
        GameObject current = system.currentSelectedGameObject;
        if (current != null && current.GetComponent<TMP_InputField>())
        {
            int index = currentInputFields.IndexOf(current.GetComponent<TMP_InputField>());
            if (index >= 0)
            {
                TMP_InputField nextField = currentInputFields[(index + 1) % currentInputFields.Count];
                system.SetSelectedGameObject(nextField.gameObject, new BaseEventData(system));
            }
        }
    }

    private void OpenPanel(GameObject panel)
    {
        ClearUI();
        panel.SetActive(true);
        currentInputFields = panelInputFields.ContainsKey(panel) ? panelInputFields[panel] : new List<TMP_InputField>();
    }

    // 이하 메서드는 필요에 따라 OpenPanel을 사용하도록 수정
    public void OpenLoginPanel() => OpenPanel(loginPanel);
    public void OpenRegistrationPanel() => OpenPanel(registrationPanel);
    public void OpenFindIDPWPanel() => OpenPanel(findIDPWPanel);
    public void OpenGamePanel() => OpenPanel(profilePanel);
    public void OpentermOfServicePanel() => OpenPanel(termOfServicePanel);

    public void ClearUI()
    {
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        profilePanel.SetActive(false);
        findIDPWPanel.SetActive(false);
        termOfServicePanel.SetActive(false);
    }



    public void ShowVerificationResponse(bool isEmailSent, string emailId, string errorMessage)
    {
        ClearUI();
        emailVerificationPanel.SetActive(true);

        if (isEmailSent)
        {
            emailVerificationText.text = $"{emailId}\n확인 이메일이 전송되었습니다. 이메일울 확인해 주세요";
        }
        else
        {
            emailVerificationText.text = $"Could't sent email : {errorMessage}";
        }
    }

    // 에러 메시지를 화면에 표시하는 메서드
    public void ShowError(string message)
    {
        errorMessageText.text = message; // TextMeshPro 텍스트에 메시지 설정
        errorPanel.SetActive(true); // 에러 패널 활성화

        Debug.LogError(message); // 중복으로 에러가 뜨는 걸 확인하기 위해 log도 띄워줌 
    }

    public void CloseError()
    {
        errorPanel.SetActive(false);
    }

    //public void MoveScene(string nextScene)
    //{
    //    SceneCtrlMananger.Instance.MoveScene(nextScene);
    //}

}
