using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionResults {
    public string id;
    public string  @object;
    public int created;
    public string model;
    public List<CompletionChoice> choices;
}
