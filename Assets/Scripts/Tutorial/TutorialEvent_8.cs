using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEvent_8 : TutorialBaseEvent
{
    public override void InitEvent()
    {
        base.InitEvent();
    }

    public override void StartEvent()
    {
        SceneManager.LoadScene("StartScene");
    }

    public override void CheckEvent()
    {

    }
}
