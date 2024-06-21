using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : Singleton<LogManager>
{

    [SerializeField]
    private Text _warningText;

    private void Start()
    {
        _warningText.gameObject.SetActive(false);
    }

    public void ShowWarningText(string message)
    {
        _warningText.text = message;

        StartCoroutine(ShowMessageAsync());
    }

    private IEnumerator ShowMessageAsync()
    {
        _warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        _warningText.gameObject.SetActive(false);
    }
}
