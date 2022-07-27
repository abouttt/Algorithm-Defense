using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;
    public float MoveSpeed;                     //카메라 이동 스피드

    private Vector3 mousePositionSaved;         //마우스 위치 저장
    private bool _leftMove =false;
    private bool _rightMove =false;
    private bool _mousMove = false;


    //public float maximumZoom;                   //최대 줌
    //public float minimumZoom;                   //최소 줌
    //private float zoomAmount = -1;              //줌 양



    void Update()
    {      
        //방향키 카메라 이동

        //왼쪽
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _leftMove = true;        
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _leftMove = false;       
        }

        //오른쪽
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _rightMove = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _rightMove = false;

        }



        if (_leftMove)
        {
            CameraLeftMove();
        }
        if (_rightMove)
        {
            CameraRightMove();
        }



       
        //마우스 카메라 이동(횔 버튼)
        if (Input.GetMouseButtonDown(2))
        {
            mousePositionSaved = Input.mousePosition;
            _mousMove = true;

        }
        if (Input.GetMouseButtonUp(2))
        {
            _mousMove = false;
       
        }


        if(_mousMove)
        {
           
            CameraMousMove();
        }


        //카메라 줌 아웃
        //zoomAmount += Input.GetAxis("Mouse ScrollWheel");// 마우스 횔 읽어오기
        //if (-zoomAmount < minimumZoom)//줌아웃
        //{
        //    zoomAmount = -minimumZoom;
        //}
        //if (-zoomAmount > maximumZoom)//줌인
        //{
        //    zoomAmount = -maximumZoom;
        //}
        //MainCamera.transform.localScale = new Vector3(-zoomAmount, -zoomAmount, 1);//적용


    }


    public void CameraLeftMove()
    {
        if (!(MainCamera.transform.position.x <= 13.5f))
        {
            MainCamera.transform.Translate(-MoveSpeed * 0.01f, 0, 0);
        }

    }



    public void CameraRightMove()
    {
        if (!(MainCamera.transform.position.x >= 35.5f))
        {
            MainCamera.transform.Translate(MoveSpeed * 0.01f, 0, 0);
        }
       
    }


    public void CameraMousMove()
    {

        Vector3 position = Camera.main.ScreenToViewportPoint((Vector3)Input.mousePosition - mousePositionSaved);
        position.y = 0f; //카메라 y축 고정

        //카메라 스피드 설정(Time)
        Vector3 move = position * (Time.deltaTime * MoveSpeed * 10);

        //왼쪽 끝
        if (MainCamera.transform.position.x <= 13.5f)
        {
            move.x += 0.1f;
            MainCamera.transform.Translate(move);

        }
        //오른쪽 끝
        else if (MainCamera.transform.position.x >= 35.5f)
        {
            move.x -= 0.1f;
            MainCamera.transform.Translate(move);
        }
        else//끝이 아닐때 이동
        {
            MainCamera.transform.Translate(move);
        }

    }

}





