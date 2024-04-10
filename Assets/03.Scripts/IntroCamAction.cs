using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class IntroCamAction : MonoBehaviour
{
    public float zoomDuration = 1.0f; // 확대/축소 애니메이션 지속 시간
    public float startSize = 5.0f; // 시작 orthographic size
    public float endSize = 10.0f; // 종료 orthographic size

    private float startTime; // 애니메이션 시작 시간

    void Start()
    {
        // 애니메이션 시작 시간 초기화
        startTime = Time.time;

        // Coroutine을 사용하여 애니메이션을 시작합니다.
        StartCoroutine(ZoomCamera(startSize, endSize, zoomDuration));
    }

    // 카메라를 확대/축소하는 Coroutine
    private IEnumerator ZoomCamera(float start, float end, float duration)
    {
        float elapsedTime = 0; // 경과 시간

        while (elapsedTime < duration)
        {
            // 경과 시간의 비율에 따라 현재 orthographic size를 계산합니다.
            float t = elapsedTime / duration;
            float newSize = Mathf.Lerp(start, end, t);
            GetComponent<Camera>().orthographicSize = newSize;

            // 경과 시간 업데이트
            elapsedTime = Time.time - startTime;

            // 다음 프레임까지 대기합니다.
            yield return null;
        }

        // 애니메이션이 끝나면 목표 orthographic size로 설정합니다.
        GetComponent<Camera>().orthographicSize = end;
    }
}
