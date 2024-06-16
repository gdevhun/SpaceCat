using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Firebase.Auth;
using Newtonsoft.Json;

public class FlaskCommunication : Singleton<FlaskCommunication>
{
    private string flaskSendUrl = "http://3.38.61.33:5000/send_data";
    private string flaskReadUrl = "http://3.38.61.33:5000/send_data";
    // 3.38.61.33
    // 위치 정보 저장을 위한 변수
    private double latitude;
    private double longitude;

    public void SendDataWithLocationAndDate(double latitude, double longitude, Action<string> callback)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        NTPClient ntpClient = new NTPClient();
        DateTime networkTime = ntpClient.GetNetworkTime();

        if (user != null)
        {
            // 사용자 데이터를 가져옴
            string userId = user.UserId;
            string userName = user.DisplayName;
            string userEmail = user.Email;
            string currentDate = networkTime.ToString("yyyyMMddHHmm");
            Debug.Log(currentDate);


            // 보낼 데이터 생성
            var data = new
            {
                userId = userId,
                userName = userName,
                userEmail = userEmail,
                // 위치 정보 (위도 경도 고도)
                location = new
                {
                    latitude = latitude,
                    longitude = longitude
                },
                datetime = currentDate
            };

            // 데이터 전송 시작
            StartCoroutine(SendDataToFlask(data, callback));
        }
        else
        {
            Debug.LogError("User not signed in.");
        }
    }

    public void SendDataWithForecast(string forecast, Action<string> callback)
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
                forecast = forecast
            };

            // 데이터 전송 시작
            StartCoroutine(SendDataToFlask(data, callback));
        }
        else
        {
            Debug.LogError("User not signed in.");
        }
    }

    // 서버 데이터 읽어 오기
    public void ReadData(string userId, Action<string> callback)
    {
        StartCoroutine(GetDataFromFlask(userId, callback));
    }

    IEnumerator SendDataToFlask(object data, Action<string> callback)
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

    IEnumerator GetDataFromFlask(string userId, Action<string> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{flaskReadUrl}/{userId}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data received successfully!");
            Debug.Log(request.downloadHandler.text);

            // JSON 응답을 파싱하여 필요한 문자열 변수만 추출
            var response = JsonConvert.DeserializeObject<ServerResponse>(request.downloadHandler.text);
            string det = response.det;
            callback(det);
        }
        else
        {
            Debug.LogError($"Error receiving data: {request.error}");
        }
    }

    [Serializable]
    public class ServerResponse
    {
        public string status;
        public Data data;
        public string det;
    }

    [Serializable]
    public class Data
    {
        public string userId;
        public string userName;
        public string userEmail;
        public Location location;
        public string date;
        public string forecast;
    }

    [Serializable]
    public class Location
    {
        public float latitude;
        public float longitude;
    }
}