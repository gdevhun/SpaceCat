using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Globalization;

public class Weather : MonoBehaviour
{
    static HttpClient client = new HttpClient();
        
    private const string ServiceKey = "b0b3Iuv4C7pVspeadXHv2qkPA75fHpZeklVYsfGYVEbhRk4spDkktlwH4ZBaY80hmCyxjlCduwwE7%2F7DN6BiFQ%3D%3D";
    private const string baseUrl = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst";

    static void weather(string date, string time)
    {
        string url = baseUrl;
        url += "?ServiceKey=" + ServiceKey;
        url += "&pageNo=1";
        url += "&numOfRows=1000";
        url += "&dataType=JSON"; 
        url += "&base_date=" + date;
        url += "&base_time=" + time;
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

            if (category == "PTY") // PTY, REH, RN1, T1H, UUU, VEC, VVV, WSD
            {
                Debug.Log("현재 강수형태: " + value);
            }

            // 추출한 정보를 출력합니다.
            Debug.Log("Category: " + category + ", Value: " + value);
        }        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        NTPClient ntpClient = new NTPClient();
        DateTime networkTime = ntpClient.GetNetworkTime();        

        string date = networkTime.ToString("yyyyMMdd");
        string time = networkTime.ToString("HHmm");

        Debug.Log("Date: " + date);
        Debug.Log("Time: " + time);

        weather(date, time);
    }        
}