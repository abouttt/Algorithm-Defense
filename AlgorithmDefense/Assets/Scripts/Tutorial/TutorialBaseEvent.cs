using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBaseEvent : MonoBehaviour
{
    public List<string> TextList;
    public List<string> FailedTextList;

    [HideInInspector]
    public bool IsSuccessEvent = false;
    [HideInInspector]
    public bool IsFailureEvent = false;

    public abstract void InitEvent();
    public abstract void CheckEvent();
}
