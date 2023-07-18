using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.Events;
using TMPro;

public class PaLM2TextPrompt : MonoBehaviour
{

    //Add to start of text to add some personality/Commands
    public string addToPromptStart =  "Be Brief";

    public TextMeshProUGUI outputText;

    [TextArea(2,5)]
    public string textOutput;

    //When Request is Sent
    public UnityEvent startRequest;
    //Request is Complete
    public UnityEvent requestComplete;
    //Request Failed
    public UnityEvent requestFailed;

    [Header("-AI SETTINGS-")]
    public string apiKey = "KEY";
    public float temperature = .7f;
    public int topK = 40;
    public float topP = .95f;
    public int candidateCount = 1;
    public int maxOutputTokens = 512;

    [Header("-Debug-")]
    public string testPrompt = "PeanutButter and ?";
    public bool debugResponse;

    //[Header("-OtherSettings-")]
    //public TTSSpeaker mySpeaker;

    //Start he Prompt Here
    public void TextPrompt(string chg)
    {
        StartCoroutine(DoTextPromptRequest(chg));
    }


    public void TextPromptFromInput(TMP_InputField myText)
    {
        TextPrompt(myText.text);
    }

    //[Button]
    public void TextPromptTest()
    {
        StartCoroutine(DoTextPromptRequest(testPrompt));
    }


    //Main REquest
    IEnumerator DoTextPromptRequest(string promptText = "Are you Online?")
    {
        startRequest.Invoke();
        //Clear output text
        OutputText("");
        //
        promptText = addToPromptStart+ " " + promptText;
        string url = "https://generativelanguage.googleapis.com/v1beta2/models/text-bison-001:generateText?key=" + apiKey;
        string requestData = "{\"prompt\":{\"text\":\"" + promptText + "\"},\"temperature\":" + temperature + ",\"top_k\":" + topK + ",\"top_p\":" + topP + ",\"candidate_count\":" + candidateCount + ",\"max_output_tokens\":" + maxOutputTokens + ",\"stop_sequences\":[],\"safety_settings\":[{\"category\":\"HARM_CATEGORY_DEROGATORY\",\"threshold\":1},{\"category\":\"HARM_CATEGORY_TOXICITY\",\"threshold\":1},{\"category\":\"HARM_CATEGORY_VIOLENCE\",\"threshold\":2},{\"category\":\"HARM_CATEGORY_SEXUAL\",\"threshold\":2},{\"category\":\"HARM_CATEGORY_MEDICAL\",\"threshold\":2},{\"category\":\"HARM_CATEGORY_DANGEROUS\",\"threshold\":2}]}";
        string contentType = "application/json";

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestData);

        //Post to PaLM2
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", contentType);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                PromptFailure("Request Failure " + www.error + " Response:" + www.downloadHandler.text);
            }
            else // Success! Output response to a string and print it out
            {
                string responseText = www.downloadHandler.text;
                //Debug.LogError(responseText);
                JSONNode parsedData = JSONNode.Parse(responseText);
                string outputText = parsedData["candidates"][0]["output"];
                if(debugResponse)
                {
                    Debug.LogError(promptText  + " : " + responseText);
                }
                textOutput = outputText;
                requestComplete.Invoke();
                OutputText(outputText);
            }
        }
    }

    void PromptFailure(string whyFail)
    {
        Debug.LogError("Error: " + whyFail);
        OutputText("Failure");
        requestFailed.Invoke();
    }


    public void OutputText(string chg)
    {
        if(outputText)
        {
            outputText.text = chg;
        }
        //Add Text to Speech Here 
        //if(mySpeaker)
        //{
        //    mySpeaker.Speak(chg);
        //}
    }

}
