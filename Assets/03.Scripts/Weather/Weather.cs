using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json.Linq;

public class Weather : MonoBehaviour
{
    static HttpClient client = new HttpClient();

    // URL과 관련된 상수
    private const string ServiceKey = "b0b3Iuv4C7pVspeadXHv2qkPA75fHpZeklVYsfGYVEbhRk4spDkktlwH4ZBaY80hmCyxjlCduwwE7%2F7DN6BiFQ%3D%3D";
    private const string baseUrl = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst";

    static void weahter()
    {
        string url = baseUrl;
        url += "?ServiceKey=" + ServiceKey;
        url += "&pageNo=1";
        url += "&numOfRows=1000";
        url += "&dataType=JSON"; // 데이터를 JSON 형식으로 요청합니다.
        url += "&base_date=20240526";
        url += "&base_time=0600";
        url += "&nx=63";
        url += "&ny=89";

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";

        string results = string.Empty;
        HttpWebResponse response;
        using (response = request.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            results = reader.ReadToEnd();
        }

        ParseWeatherData(results);
    }

    static void ParseWeatherData(string jsonData)
    {
        // JSON 데이터를 파싱합니다.
        JObject weatherData = JObject.Parse(jsonData);

        // item 배열을 반복하여 모든 항목을 출력합니다.
        foreach (var item in weatherData["response"]["body"]["items"]["item"])
        {
            string category = item["category"].ToString();
            string value = item["obsrValue"].ToString();

            // 추출한 정보를 출력합니다.
            Debug.Log("Category: " + category + ", Value: " + value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        weahter();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
