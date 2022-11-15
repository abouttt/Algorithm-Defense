using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StageAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI stageText;


    private void Start()
    {
        int num = PlayerPrefs.GetInt("Num");

        stageText.text = "Stage "+ num.ToString();

    }



}
