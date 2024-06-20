using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;
using TMPro;

public class ChatGPTManager : MonoBehaviour
{
    public OnRespeonseEvent OnResponse;
    public TMP_Text OnContent;
    public TMP_InputField InputContent;

    [SerializeField]
    private string _userMBTI;

    [System.Serializable]
    public class OnRespeonseEvent : UnityEvent<string> {  } 
    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    public async void AskChatGPT(string newText)
    {
         _userMBTI = FirebaseReadingManager.Instance.CurrentUserMBTI;

        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        //request.Model = "gpt-3.5-turbo";     // 기존 모델
        if (_userMBTI == "ENFP") request.Model = "ft:gpt-3.5-turbo-1106:personal:enfp:9a7z3UGy";
        else if (_userMBTI == "ISTJ") request.Model = "ft:gpt-3.5-turbo-1106:personal:istj:9ae5tbCg";
        else if (_userMBTI == "ISFJ") request.Model = "ft:gpt-3.5-turbo-1106:personal:isfj:9aePUAFX";
        else if (_userMBTI == "INFJ") request.Model = "ft:gpt-3.5-turbo-1106:personal:infj:9afvwyb8";
        else if (_userMBTI == "INTJ") request.Model = "ft:gpt-3.5-turbo-1106:personal:intj:9agAyMa2";
        else if (_userMBTI == "ISTP") request.Model = "ft:gpt-3.5-turbo-1106:personal:istp:9afcFj1t";
        else if (_userMBTI == "ISFP") request.Model = "ft:gpt-3.5-turbo-1106:personal:isfp:9agRwcYk";
        else if (_userMBTI == "INFP") request.Model = "ft:gpt-3.5-turbo-1106:personal:infp:9aj27MNT";
        else if (_userMBTI == "INTP") request.Model = "ft:gpt-3.5-turbo-1106:personal:intp:9agmgDm6";
        else if (_userMBTI == "ESTP") request.Model = "ft:gpt-3.5-turbo-1106:personal:estp:9adgiQBX";
        else if (_userMBTI == "ESFP") request.Model = "ft:gpt-3.5-turbo-1106:personal:esfp:9ahHKiCc";
        else if (_userMBTI == "ENTP") request.Model = "ft:gpt-3.5-turbo-1106:personal:entp:9aim7uAJ";
        else if (_userMBTI == "ESTJ") request.Model = "ft:gpt-3.5-turbo-1106:personal:estj:9afFIqJc";
        else if (_userMBTI == "ESFJ") request.Model = "ft:gpt-3.5-turbo-1106:personal:esfj:9ajJlNX4";
        else if (_userMBTI == "ENFJ") request.Model = "ft:gpt-3.5-turbo-1106:personal:enfj:9ajwxlK0";
        else if (_userMBTI == "ENTJ") request.Model = "ft:gpt-3.5-turbo-1106:personal:entj:9akKfCgv";

        var response = await openAI.CreateChatCompletion(request);

        if(response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            Debug.Log("chatResponse: " + chatResponse.Content);

            InputContent.text = string.Empty;
            InputContent.ActivateInputField();
            InputContent.Select();
            OnContent.text = newText;
            OnResponse.Invoke(chatResponse.Content);
        }

    }
}
