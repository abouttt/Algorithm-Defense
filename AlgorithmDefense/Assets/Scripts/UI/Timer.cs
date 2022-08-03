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
        //타이머 초기화(초 단위)
        _currentTime = 60;
    }


    void Update()
    {
     
        // 메뉴창
        if (Input.GetButtonDown("Cancel"))//esc버튼 클릭
        {

            if (_timerActive)
            {
                _timerActive = false;

            }
            else//아니면
            {
                _timerActive = true;
            }

        }

        //시간이 흐를 때
        if (_timerActive == true)
        {
            _currentTime = _currentTime - Time.deltaTime;

            //타이머가 0이 되었다면
            if (_currentTime <= 0)
            {
                //초기화
                _currentTime = 60;

                //웨이브 증가
                WaveCount.WaveCountPlus();
                //다음 웨이브 호출

            }

            //시간단위로 변경
            TimeSpan time = TimeSpan.FromSeconds(_currentTime);
            //text에 시간 입력
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
