using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Auth;
using Newtonsoft.Json;

public class FlaskCommunication : MonoBehaviour
{
    private string flaskUrl = "http://localhost:5000/send_data";

    void Start()
    {
        // Firebase 사용자 인증 정보를 가져온 후 데이터 전송
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            // 토큰을 가져와서 데이터를 전송
            StartCoroutine(GetTokenAndSendData(user));
        }
        else
        {
            Debug.LogError("User not signed in.");
        }
    }

    IEnumerator GetTokenAndSendData(FirebaseUser user)
    {
        var task = user.TokenAsync(true);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError("Failed to get token: " + task.Exception);
            yield break;
        }

        string idToken = task.Result;

        // 보낼 데이터 생성
        var data = new
        {
            idToken = idToken,
            customData = new { message = "Hello from Unity" } // 추가적인 데이터 포함
        };

        // 데이터 전송 시작
        StartCoroutine(SendDataToFlask(data));
    }

    IEnumerator SendDataToFlask(object data)
    {
        string json = JsonConvert.SerializeObject(data);
        UnityWebRequest request = new UnityWebRequest(flaskUrl, "POST");
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
}
