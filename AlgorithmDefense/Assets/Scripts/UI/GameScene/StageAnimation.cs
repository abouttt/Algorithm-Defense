using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageAnimation : MonoBehaviour
{
    [SerializeField]
    private Text stageText;


    private void Start()
    {
        int num = PlayerPrefs.GetInt("Num");

        stageText.text = "Stage "+ num.ToString();

    }



}
