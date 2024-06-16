using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Auth;
using Newtonsoft.Json;

public class FlaskCommunication : Singleton<FlaskCommunication>
{
    private string flaskSendUrl = "http://localhost:5000/send_data";
    private string flaskReadUrl = "http://localhost:5000/read_data";

    public void SendData()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            // 사용자 데이터를 가져옴
            string userId = user.UserId;
            string userName = user.DisplayName;
            string userEmail = user.Email;

            // 보낼 데이터 생성
            var data = new
            {
                userId = userId,
                userName = userName,
                userEmail = userEmail,
            };

            // 데이터 전송 시작
            StartCoroutine(SendDataToFlask(data));
        }
        else
        {
            Debug.LogError("User not signed in.");
        }
    }

    public void ReadData(string userId)
    {
        StartCoroutine(GetDataFromFlask(userId));
    }

    IEnumerator SendDataToFlask(object data)
    {
        string json = JsonConvert.SerializeObject(data);
        UnityWebRequest request = new UnityWebRequest(flaskSendUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data sent successfully!");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Error sending data: {request.error}");
        }
    }

    IEnumerator GetDataFromFlask(string userId)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{flaskReadUrl}/{userId}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data received successfully!");
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Error receiving data: {request.error}");
        }
    }
}
