using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Google;
using TMPro;

public class GoogleAuthentication : MonoBehaviour
{
    public string imageURL;

    public TMP_Text userNameTxt, userEmailTxt;

    public GameObject loginPanel;

    public string webClientId = "1077253468932-j3e7ushv5trp8cgo0ss0dumh0l8j6518.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
    }

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished, TaskScheduler.Default);
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.LogError("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Canceled");
        }
        else
        {
            Debug.LogError("Welcome: " + task.Result.DisplayName + "!");

            userNameTxt.text = "" + task.Result.DisplayName;
            userEmailTxt.text = "" + task.Result.Email;

            imageURL = task.Result.ImageUrl.ToString();
            loginPanel.SetActive(false); 
        }
    }


    public void OnSignOut()
    {
        userNameTxt.text = "";
        userEmailTxt.text = "";

        imageURL = "";
        loginPanel.SetActive(true);

        Debug.LogError("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }
}
