using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Exit : MonoBehaviour
{
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        // 버튼의 클릭 이벤트에 OnLoadScene 메서드 등록
        if (_exitButton != null)
        {
            _exitButton.onClick.AddListener(() => OnLoadScene().Forget());
        }
    }

    private async UniTaskVoid OnLoadScene()
    {
        await UniTask.Yield();

        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("02.LoginScene");
        loadSceneAsync.allowSceneActivation = false;

        while (!loadSceneAsync.isDone)
        {
            await UniTask.Yield();

            if (loadSceneAsync.progress >= 0.9f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));

                loadSceneAsync.allowSceneActivation = true;
            }
        }
    }
}
