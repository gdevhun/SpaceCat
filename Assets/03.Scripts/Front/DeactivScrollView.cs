using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivScrollView : MonoBehaviour
{
    public GameObject ExitBtn;
    public GameObject ScrollView;

    public void ClickExit()
    {
        ScrollView.SetActive(false);
    }
}
