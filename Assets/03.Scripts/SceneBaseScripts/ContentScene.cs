using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentScene : MonoBehaviour
{
    public TextMeshProUGUI mbtiText;

    private void Start()
    {
        mbtiText.text = TestResult.Instance.ShowResult();
    }
}
