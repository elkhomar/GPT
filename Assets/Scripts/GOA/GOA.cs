using Core.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GOA : Singleton<GOA>
{
    public string APIKey;

    private string modelsURL = "https://api.openai.com/v1/models";
    private string completionsURL = "https://api.openai.com/v1/completions";
    private string fileUploadURL = "https://api.openai.com/v1/files";
    private string fineTuneURL = "https://api.openai.com/v1/fine-tunes";
    private string checkFineTuneURL = "https://api.openai.com/v1/fine-tunes/";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetModels() {
        StartCoroutine(InternalGetModels());
    }

    IEnumerator InternalGetModels() {

        using (UnityWebRequest www = UnityWebRequest.Get(modelsURL)) {
            www.SetRequestHeader("Authorization", "Bearer " + APIKey);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log("Error: " + www.error);
            }
            else {
                Debug.Log(":\nReceived: " + www.downloadHandler.text);
            }
        }
    }

    public void CreateCompletion(string prompt,string customModel, System.Action< CompletionResults> callback) {
        StartCoroutine(InternalCreateCompletion(prompt, customModel,callback));
    }

    public void UploadFile(string filePath, string shortName, System.Action<UploadFileResults> callback) {
        StartCoroutine(InternalUploadFile(filePath, shortName, callback));
    }


    public void FineTune(string fileID, System.Action<FineTuneResults> callback) {
        StartCoroutine(InternalFineTune(fileID, callback));
    }

    public void CheckFineTune(string fineTuneID, System.Action<CheckFineTuneResults> callback) {
        StartCoroutine(InternalCheckFineTune(fineTuneID, callback));
    }

    IEnumerator InternalCheckFineTune(string fineTuneID, System.Action<CheckFineTuneResults> callback) {


        using (UnityWebRequest www = UnityWebRequest.Get(checkFineTuneURL + fineTuneID)) {
            www.SetRequestHeader("Authorization", "Bearer " + APIKey);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log("Error: " + www.error);
            }
            else {
                Debug.Log(":\nReceived: " + www.downloadHandler.text);
                CheckFineTuneResults cr = JsonUtility.FromJson<CheckFineTuneResults>(www.downloadHandler.text);
                callback(cr);
            }
        }

    }
    IEnumerator InternalFineTune(string fileID, System.Action<FineTuneResults> callback) {
        var user = new FineTuneArgs();
        user.training_file = fileID;
        string json = JsonUtility.ToJson(user);


        using (UnityWebRequest www = UnityWebRequest.Post(fineTuneURL, "POST")) {
            www.SetRequestHeader("Authorization", "Bearer " + APIKey);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log("Error: " + www.error);
            }
            else {
                Debug.Log(":\nReceived: " + www.downloadHandler.text);
                FineTuneResults cr = JsonUtility.FromJson<FineTuneResults>(www.downloadHandler.text);
                callback(cr);
            }
        }
    }

        IEnumerator InternalUploadFile(string filePath, string shortName, System.Action<UploadFileResults> callback) {

        var form = new WWWForm();
        form.AddField("purpose", "fine-tune");

        byte[] byteArray = System.IO.File.ReadAllBytes(filePath);
        form.AddBinaryData("file", byteArray, shortName, "text/plain");
        UnityWebRequest www = UnityWebRequest.Post(fileUploadURL,form);
        www.SetRequestHeader("Authorization", "Bearer " + APIKey);

        //var user = new UploadFileArgs();
        //user.file = filePath;
        //user.purpose = "fine-tune";
        //string json = JsonUtility.ToJson(user);


        //using (UnityWebRequest www = UnityWebRequest.Post(fileUploadURL, "POST")) {
        //    www.SetRequestHeader("Authorization", "Bearer " + APIKey);
        //    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        //    www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        //    www.SetRequestHeader("Content-Type", "application/json");



        yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log("Error: " + www.error);
            }
            else {
                Debug.Log(":\nReceived: " + www.downloadHandler.text);
                UploadFileResults cr = JsonUtility.FromJson<UploadFileResults>(www.downloadHandler.text);
                callback(cr);
            }
        //}
    }

    IEnumerator InternalCreateCompletion(string prompt,string customModel,System.Action< CompletionResults> callback) {

        var user = new CompletionArgs();
        user.prompt = prompt;

        if (customModel != "")
            user.model = customModel;
        else
            user.model = "text-davinci-002";

        user.temperature = 0.6f;
        user.max_tokens = 20;
        string json = JsonUtility.ToJson(user);


        using (UnityWebRequest www = UnityWebRequest.Post(completionsURL,"POST")) {
            www.SetRequestHeader("Authorization", "Bearer " + APIKey);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log("Error: " + www.error);
            }
            else {
                Debug.Log(":\nReceived: " + www.downloadHandler.text);
                CompletionResults cr = JsonUtility.FromJson<CompletionResults>(www.downloadHandler.text);
                callback(cr);
            }
        }
    }
    }
