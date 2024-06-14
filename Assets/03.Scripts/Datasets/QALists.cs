using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QALists : Singleton<QALists>
{
    public string[] _questionStrings; //저장할 질문 스트링
    public string[] _answerString1; //저장할 답 스트링1
    public string[] _answerString2; //저장할 답 스트링2

    public bool isInfoFetchCompleted = false;

    private static QALists _instance;

    private void Awake()
    {
        _questionStrings = new string[36];
        _answerString1 = new string[36];
        _answerString2 = new string[36];

        if (_instance == null)
        {
            _instance = this as QALists;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
}

    private void Start()
    {
        StartCoroutine(WaitFetchTime());
    }

    private IEnumerator WaitFetchTime()
    {
        // fireReadingManager의 FirebaseReadingManager 컴포넌트가 준비될 때까지 대기
        yield return new WaitUntil(() =>
            //FirebaseReadingManager.Instance.isTestInfoFetchCompleted == true);
            isInfoFetchCompleted == true);
        // 조건이 충족되면 UI를 업데이트
    }
    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    _questionStrings = new string[36];
    //    _answerString1 = new string[36];
    //    _answerString2 = new string[36];
    //}
}
