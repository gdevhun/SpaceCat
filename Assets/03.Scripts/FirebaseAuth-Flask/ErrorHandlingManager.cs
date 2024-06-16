using UnityEngine;

public static class ErrorHandlingManager
{
    // 입력 오류에 대한 처리
    public static bool ValidateInput(string input, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            UIManagerAuth.Instance.ShowError(errorMessage);
            return false;
        }
        return true;
    }

    // 서버 응답 오류 처리
    public static void HandleLoginError(string errorMessage)
    {
        string message = "오류 발생: " + errorMessage;
        switch (errorMessage)
        {
            case "Email and password required":
                message = "이메일과 비밀번호가 필요합니다. 입력해 주세요.";
                break;
            case "Invalid email or password":
                message = "이메일 또는 비밀번호가 잘못되었습니다. 다시 시도해 주세요.";
                break;
            // 추가적인 오류 메시지에 대한 케이스는 여기에 추가
            default:
                message = "알 수 없는 오류가 발생했습니다. 오류 메시지: " + errorMessage;
                break;
        }

        UIManagerAuth.Instance.ShowError(message);
        Debug.LogError(message);
    }
}
