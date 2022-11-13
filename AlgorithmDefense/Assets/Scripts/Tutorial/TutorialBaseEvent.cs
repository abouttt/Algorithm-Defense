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

    private GameObject _guideUI;

    private void Start()
    {
        _guideUI = transform.Find("GuideUI").gameObject;
    }

    public abstract void InitEvent();
    public abstract void StartEvent();
    public abstract void CheckEvent();

    public void SetActiveGuideUI(bool isShow)
    {
        if (_guideUI)
        {
            _guideUI.SetActive(isShow);
        }
    }
}
