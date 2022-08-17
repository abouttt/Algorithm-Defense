using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Wave : MonoBehaviour
{
    [SerializeField]
    private Text WaveText;
    private int _count;


    private void Start()
    {
        _count = 0;
        WaveText.text = "Wave waitng";
    }


    public void WaveCountPlus()
    {
        _count++;
        WaveText.text = "Wave "+_count.ToString();

        //if 6라운드면 정지

    }



}
