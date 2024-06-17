using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GPS
{
    public class Location : MonoBehaviour
    {
        public static double first_Lat; //최초 위도
        public static double first_Long; //최초 경도
        public static double current_Lat = 35.8510; //현재 위도 고정
        public static double current_Long = 127.1263; //현재 경도 고정
        public static Dictionary<string, double> result;

        private static WaitForSeconds second = new WaitForSeconds(1);

        [SerializeField]
        public TextMeshProUGUI txtMain;
        public TextMeshProUGUI txtMain2;

        public void StartGPS()
        {
            StartCoroutine(StartGPSCoroutine());
        }

        private IEnumerator StartGPSCoroutine()
        {
            yield return second;

            // 고정된 현재 위치 사용
            Debug.Log(current_Lat);
            Debug.Log(current_Long);

            // 위치 전송
            FlaskCommunication.Instance.SendDataWithLocationAndDate(current_Lat, current_Long, emptyFunction);

            yield return second;

            StartCoroutine(ExecuteReadDataAfterDelay(5f));
        }

        private IEnumerator ExecuteReadDataAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            ReadData();
        }

        public void ReadData()
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            FirebaseUser user = auth.CurrentUser;
            // 데이터를 읽기
            FlaskCommunication.Instance.ReadData(user.UserId, emptyFunction);
        }

        public static Dictionary<string, double> GetXY()
        {
            // 결과 반환 시, result["x 또는 y"];로 호출
            return result;
        }

        public void emptyFunction(string det)
        {
            Debug.Log(det);
            txtMain.text = det;
            txtMain2.text = det;
        }
    }
}
