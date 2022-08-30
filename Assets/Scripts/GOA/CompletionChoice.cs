using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompletionChoice {
    public string text;
    public int index;
    //public int logprobs;
    public string finish_reason;
}
