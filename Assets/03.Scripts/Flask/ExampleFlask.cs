using UnityEngine;
using Firebase.Auth;

public class ExampleFlask : MonoBehaviour
{
    public void SendLocationAndDate()
    {
        // 날짜와 위치 정보를 포함한 데이터를 보내기
        FlaskCommunication.Instance.SendDataWithLocationAndDate(37.7749, -122.4194);
    }

    public void SendForecast()
    {
        // 예보 정보를 포함한 데이터를 보내기
        FlaskCommunication.Instance.SendDataWithForecast("Sunny with a chance of rain");
    }

    public void ReadData()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        // 데이터를 읽기
        FlaskCommunication.Instance.ReadData(user.UserId);
    }
}
