using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;
    public float MoveSpeed = 10;                     //카메라 이동 스피드

    private Vector3 mousePositionSaved;         //마우스 위치 저장


    public float maximumZoom = 0f;                   //최대 줌
    public float minimumZoom = 0f;                   //최소 줌
    public float zoomSpeed = 0f;              //줌 양



    void Update()
    {
        //방향키 카메라 이동

        //왼쪽
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            CameraLeftMove();
        }

        //오른쪽
        if (Input.GetKey(KeyCode.RightArrow))
        {
            CameraRightMove();
        }


        if (Input.GetMouseButton(2))
        {

            CameraMousMove();

        }


        //if(_mousMove)
        //{

        //    CameraMousMove();
        //}


        //CameraZoom();



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

        float t_posX = Input.GetAxis("Mouse X");


        //왼쪽 끝
        if (MainCamera.transform.position.x < 13.5f)
        {
            MainCamera.transform.position = new Vector3(13.5f, 7.5f, -10f);

        }
        //오른쪽 끝
        else if (MainCamera.transform.position.x > 35.5f)
        {
            MainCamera.transform.position = new Vector3(35.5f, 7.5f, -10f);

        }
        else//끝이 아닐때 이동
        {
            MainCamera.transform.position -= new Vector3(t_posX, 0, 0);

        }



        //Vector3 position = Camera.main.ScreenToViewportPoint((Vector3)Input.mousePosition - mousePositionSaved);
        //position.y = 0f; //카메라 y축 고정

        ////카메라 스피드 설정(Time)
        //Vector3 move = position * (Time.deltaTime * MoveSpeed * 10);

        ////왼쪽 끝
        //if (MainCamera.transform.position.x <= 13.5f)
        //{
        //    move.x += 0.1f;
        //    MainCamera.transform.Translate(move);

        //}
        ////오른쪽 끝
        //else if (MainCamera.transform.position.x >= 35.5f)
        //{
        //    move.x -= 0.1f;
        //    MainCamera.transform.Translate(move);
        //}
        //else//끝이 아닐때 이동
        //{
        //    MainCamera.transform.Translate(move);
        //}

    }


    public void CameraZoom()
    {
        //카메라 줌 아웃
        float t_zoomDirection = Input.GetAxis("Mouse ScrollWheel"); // 마우스 횔 읽어오기

        if (transform.position.y <= maximumZoom && t_zoomDirection > 0)
        {
            return;
        }
        if (transform.position.y >= minimumZoom && t_zoomDirection < 0)
        {
            return;
        }

        MainCamera.orthographicSize += t_zoomDirection * MoveSpeed;
    }


}





