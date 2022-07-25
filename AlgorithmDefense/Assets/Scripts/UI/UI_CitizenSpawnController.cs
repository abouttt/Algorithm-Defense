using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System;


public class UI_CitizenSpawnController : CitizenSpawner
{

    public Text RedButtenText;
    public Text BlueButtenText;
    public Text GreenButtenText;
    public Text YellowButtenText;


    



    private bool _onRed;
    private bool _onGreen;
    private bool _onBlue;
    private bool _onYellow;


    private void Start()
    {
        RedButtenText.text = "OFF";
        BlueButtenText.text = "OFF";
        GreenButtenText.text = "OFF";
        YellowButtenText.text = "OFF";


        _onRed = false;
        _onGreen = false;
        _onBlue = false;
        _onYellow = false;

    }



    public void OnButtonRed()
    {
        CitizenSpawner.GetInstance.SetOnOff(Define.Citizen.Red);
        if (!_onRed)
        {
            RedButtenText.text = "ON";
            _onRed = true;
        }
        else
        {
            RedButtenText.text = "OFF";
            _onRed = false;
        }

    }

    public void OnButtonBlue()
    {
        CitizenSpawner.GetInstance.SetOnOff(Define.Citizen.Blue);
        if (!_onBlue)
        {
            BlueButtenText.text = "ON";
            _onBlue = true;
        }
        else
        {
            BlueButtenText.text = "OFF";
            _onBlue = false;
        }

    }

    public void OnButtonGreen()
    {
        CitizenSpawner.GetInstance.SetOnOff(Define.Citizen.Green);
        if (!_onGreen)
        {
            GreenButtenText.text = "ON";
            _onGreen = true;
        }
        else
        {
            GreenButtenText.text = "OFF";
            _onGreen = false;
        }

    }
    public void OnButtonYellow()
    {
        CitizenSpawner.GetInstance.SetOnOff(Define.Citizen.Yellow);
        if (!_onYellow)
        {
            YellowButtenText.text = "ON";
            _onYellow = true;
        }
        else
        {
            YellowButtenText.text = "OFF";
            _onYellow = false;
        }

    }




    



}
