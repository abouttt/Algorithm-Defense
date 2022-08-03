using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{

    [SerializeField]
    private Wave WaveCount;
    [SerializeField]
    private Text TimeText;
    private float _currentTime;
    private bool _timerActive = true;
   


    void Start()
    {
        //Ÿ�̸� �ʱ�ȭ(�� ����)
        _currentTime = 60;
    }


    void Update()
    {
     
        // �޴�â
        if (Input.GetButtonDown("Cancel"))//esc��ư Ŭ��
        {

            if (_timerActive)
            {
                _timerActive = false;

            }
            else//�ƴϸ�
            {
                _timerActive = true;
            }

        }

        //�ð��� �带 ��
        if (_timerActive == true)
        {
            _currentTime = _currentTime - Time.deltaTime;

            //Ÿ�̸Ӱ� 0�� �Ǿ��ٸ�
            if (_currentTime <= 0)
            {
                //�ʱ�ȭ
                _currentTime = 60;

                //���̺� ����
                WaveCount.WaveCountPlus();
                //���� ���̺� ȣ��

            }

            //�ð������� ����
            TimeSpan time = TimeSpan.FromSeconds(_currentTime);
            //text�� �ð� �Է�
            TimeText.text = time.ToString(@"mm\:ss");


          
        }

    }


    public void TimeStopPush()
    {
        _timerActive = false;
    }

    public void TimeAgainPush()
    {
        _timerActive = true;
    }



}
