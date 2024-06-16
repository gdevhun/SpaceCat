using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;

public class RequestHandler : MonoBehaviour
{
    private static RequestHandler instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // RequestHandler 오브젝트가 씬 전환 시에도 유지되도록 함
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static IEnumerator PostRequest(string url, object jsonData, System.Action<string> callback)
    {
        if (instance == null)
        {
            Debug.LogError("RequestHandler instance not found.");
            yield break;
        }

        string json = JsonConvert.SerializeObject(jsonData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
        else
        {
            callback?.Invoke(request.downloadHandler.text);
        }
    }
}
