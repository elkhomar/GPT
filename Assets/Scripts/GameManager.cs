using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_InputField inputField;
    public TMP_InputField inputFieldCustomModel;

    private string currentFineTuneJob;
    void Start()
    {
        //GOA.instance.GetModels();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnButtonAskOpenAI() {
        GOA.instance.CreateCompletion(inputField.text, inputFieldCustomModel.text, onCompletionReturns) ;
    }


    public void onCompletionReturns(CompletionResults cr) {
        Debug.Log(cr.choices[0].text);
        inputField.text += cr.choices[0].text;
    }


    public void OnButtonAddFile() {
        Debug.Log("Add File");
        string path = Application.persistentDataPath + "/mydata.jsonl";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("{ \"prompt\":\"npc37 is sad\", \"completion\":\"yes\"}");
        writer.WriteLine("{ \"prompt\":\"npc35 is sad\", \"completion\":\"no\"}");
        writer.Close();

        GOA.instance.UploadFile(path, "mydata.jsonl", onFileUploadReturns);
    }

    private void onFileUploadReturns(UploadFileResults obj) {
        Debug.Log("onFileUploadReturns");
        GOA.instance.FineTune(obj.id, onFineTuneReturns);
    }

    private void onFineTuneReturns(FineTuneResults obj) {
        Debug.Log("onFineTuneReturns "+ obj.id);
        currentFineTuneJob = obj.id;
    }

    public void OnButtonCheckFineTune() {
        GOA.instance.CheckFineTune(currentFineTuneJob, OnCheckFineTuneReturns);
    }

    private void OnCheckFineTuneReturns(CheckFineTuneResults obj) {
        Debug.Log("OnCheckFineTuneReturns ");
    }

    public void OnButtonGetModels() {
        GOA.instance.GetModels();
    }
}
