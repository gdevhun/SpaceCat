using UnityEngine;
using Firebase.Auth;

public class ExampleFlask : MonoBehaviour
{
    public void SendData()
    {
        FlaskCommunication.Instance.SetLocation(1, 1, 1);
        // 데이터를 보내기
        FlaskCommunication.Instance.SendData();
    }

    public void ReadData()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        // 데이터를 읽기
        FlaskCommunication.Instance.ReadData(user.UserId);
    }
}
