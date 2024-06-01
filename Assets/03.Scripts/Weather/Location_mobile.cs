using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Location : MonoBehaviour
{
    public static double first_Lat; //최초 위도
    public static double first_Long; //최초 경도
    public static double current_Lat; //현재 위도
    public static double current_Long; //현재 경도

    private static WaitForSeconds second = new WaitForSeconds(1);
     
    private static LocationInfo location;

    IEnumerator Start()
    {
        yield return second;

        //  유저 권한 요청
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            yield return null;
            Permission.RequestUserPermission(Permission.FineLocation);            
        }

        // 유저가 GPS 사용중인지 최초 체크
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS is not enabled");
            yield break;
        }

        //GPS 서비스 시작
        Input.location.Start();
        Debug.Log("Awaiting initialization");

        //활성화될 때 까지 대기
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return second;
            maxWait -= 1;
        }

        //20초 지날경우 활성화 중단
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        //연결 실패
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;

        }
        else
        {            
            //현재 위치 갱신                        
            yield return second;
            location = Input.location.lastData;

            current_Lat = location.latitude;
            current_Long = location.longitude;
            Debug.Log(current_Lat);
            Debug.Log(current_Long);
            yield return second;            
        }
    }

    //위치 서비스 종료
    public static void StopGPS()
    {
        if (Input.location.isEnabledByUser)
        {            
            Input.location.Stop();
        }
    } 
}
