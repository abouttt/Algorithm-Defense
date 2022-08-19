using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;
    public float MoveSpeed = 10;                     //ī�޶� �̵� ���ǵ�

    private Vector3 mousePositionSaved;         //���콺 ��ġ ����


    public float maximumZoom = 0f;                   //�ִ� ��
    public float minimumZoom = 0f;                   //�ּ� ��
    public float zoomSpeed = 0f;              //�� ��



    void Update()
    {
        //����Ű ī�޶� �̵�

        //����
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            CameraLeftMove();
        }

        //������
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


        //���� ��
        if (MainCamera.transform.position.x < 13.5f)
        {
            MainCamera.transform.position = new Vector3(13.5f, 7.5f, -10f);

        }
        //������ ��
        else if (MainCamera.transform.position.x > 35.5f)
        {
            MainCamera.transform.position = new Vector3(35.5f, 7.5f, -10f);

        }
        else//���� �ƴҶ� �̵�
        {
            MainCamera.transform.position -= new Vector3(t_posX, 0, 0);

        }



        //Vector3 position = Camera.main.ScreenToViewportPoint((Vector3)Input.mousePosition - mousePositionSaved);
        //position.y = 0f; //ī�޶� y�� ����

        ////ī�޶� ���ǵ� ����(Time)
        //Vector3 move = position * (Time.deltaTime * MoveSpeed * 10);

        ////���� ��
        //if (MainCamera.transform.position.x <= 13.5f)
        //{
        //    move.x += 0.1f;
        //    MainCamera.transform.Translate(move);

        //}
        ////������ ��
        //else if (MainCamera.transform.position.x >= 35.5f)
        //{
        //    move.x -= 0.1f;
        //    MainCamera.transform.Translate(move);
        //}
        //else//���� �ƴҶ� �̵�
        //{
        //    MainCamera.transform.Translate(move);
        //}

    }


    public void CameraZoom()
    {
        //ī�޶� �� �ƿ�
        float t_zoomDirection = Input.GetAxis("Mouse ScrollWheel"); // ���콺 Ȼ �о����

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





