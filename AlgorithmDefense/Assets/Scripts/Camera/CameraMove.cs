using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;
    public float MoveSpeed;                     //ī�޶� �̵� ���ǵ�

    private Vector3 mousePositionSaved;         //���콺 ��ġ ����
    private bool _leftMove =false;
    private bool _rightMove =false;
    private bool _mousMove = false;


    //public float maximumZoom;                   //�ִ� ��
    //public float minimumZoom;                   //�ּ� ��
    //private float zoomAmount = -1;              //�� ��



    void Update()
    {      
        //����Ű ī�޶� �̵�

        //����
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _leftMove = true;        
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _leftMove = false;       
        }

        //������
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



       
        //���콺 ī�޶� �̵�(Ȼ ��ư)
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


        //ī�޶� �� �ƿ�
        //zoomAmount += Input.GetAxis("Mouse ScrollWheel");// ���콺 Ȼ �о����
        //if (-zoomAmount < minimumZoom)//�ܾƿ�
        //{
        //    zoomAmount = -minimumZoom;
        //}
        //if (-zoomAmount > maximumZoom)//����
        //{
        //    zoomAmount = -maximumZoom;
        //}
        //MainCamera.transform.localScale = new Vector3(-zoomAmount, -zoomAmount, 1);//����


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
        position.y = 0f; //ī�޶� y�� ����

        //ī�޶� ���ǵ� ����(Time)
        Vector3 move = position * (Time.deltaTime * MoveSpeed * 10);

        //���� ��
        if (MainCamera.transform.position.x <= 13.5f)
        {
            move.x += 0.1f;
            MainCamera.transform.Translate(move);

        }
        //������ ��
        else if (MainCamera.transform.position.x >= 35.5f)
        {
            move.x -= 0.1f;
            MainCamera.transform.Translate(move);
        }
        else//���� �ƴҶ� �̵�
        {
            MainCamera.transform.Translate(move);
        }

    }

}





