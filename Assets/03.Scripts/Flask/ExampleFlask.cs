using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;

public class ExampleFlask : MonoBehaviour
{
    public void SendLocationAndDate()
    {
        // 날짜와 위치 정보를 포함한 데이터를 보내기
        FlaskCommunication.Instance.SendDataWithLocationAndDate(37.7749, -122.4194, HandleExtraInfo);
    }

    public void SendForecast()
    {
        // 예보 정보를 포함한 데이터를 보내기
        FlaskCommunication.Instance.SendDataWithForecast("Sunny with a chance of rain", HandleExtraInfo);
    }

    public void ReadData()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            // 데이터를 읽기
            FlaskCommunication.Instance.ReadData(user.UserId, HandleExtraInfo);
        }
        else
        {
            Debug.LogError("User not signed in.");
        }
    }

    private void HandleExtraInfo(string det)
    {
        // det 관련 로직을 여기에 적으시면 됨.
    }
}
