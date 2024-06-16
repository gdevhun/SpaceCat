using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Auth;
using Newtonsoft.Json;

public class FlaskCommunication : Singleton<FlaskCommunication>
{
    private string flaskSendUrl = "http://3.38.61.33:5000/send_data";
    private string flaskReadUrl = "http://3.38.61.33:5000/read_data";

    // 위치 정보 저장을 위한 변수
    private double latitude;
    private double longitude;
    private double altitude;

    // 위치 정보를 설정하는 메소드
    public void SetLocation(double lat, double lon, double alt)
    {
        latitude = lat;
        longitude = lon;
        altitude = alt;
    }

    // 데이터 서버에 전달
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
            string currentDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            // 보낼 데이터 생성
            var data = new
            {
                userId = userId,
                userName = userName,
                userEmail = userEmail,
                location = new
                {
                    latitude = latitude,
                    longitude = longitude,
                    altitude = altitude
                },
                date = currentDate
            };

            // 데이터 전송 시작
            StartCoroutine(SendDataToFlask(data));
        }
        else
        {
            Debug.LogError("User not signed in.");
        }
    }

    // 서버 데이터 읽어 오기
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