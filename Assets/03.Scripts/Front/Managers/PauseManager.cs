using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private Button _button;

    private async void Start()
    {
        _button.interactable = false;
        await UniTask.Delay(1000);
        _button.interactable = true;
    }
}
