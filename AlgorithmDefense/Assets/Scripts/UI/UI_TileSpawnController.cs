using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_TileSpawnController : MonoBehaviour
{


    //�ش� ��ư�� ������ ����
    [System.Serializable]
    public class BuildButtonsInformation
    {
        public Sprite ButtonSprite; //��ư �̹���
        public int _cost;           //����
        [HideInInspector]
        public Text CostText;       //���� �ؽ�Ʈ
        [HideInInspector]
        public Button ButtonObj;   //������ ��ư ������Ʈ ����
    }

    public BuildButtonsInformation[] BuildButtons;


    [SerializeField]
    private Button TileButton;
    [SerializeField]
    private RectTransform buildButtonContainer;

    private GameObject _buildTileMenu;


    private void Start()
    {

        //�ش� ������Ʈ�� �θ� ã�Ƽ� ã��
        _buildTileMenu = GameObject.Find("BuildSpawnButtons");

        CreateButton(buildButtonContainer, TileButton, BuildButtons);

    }



    public void CreateButton(RectTransform container, Button btn_Obj, BuildButtonsInformation[] btn_Slot)
    {

        for (int i = 0; i < btn_Slot.Length; i++)
        {
            int index = i;


            //��ư ����
            Button newButton = Instantiate(btn_Obj, container.transform);
            btn_Slot[i].ButtonObj = newButton;


            //������ ��ư�� ��ǥ�� �̹��� ����
            newButton.transform.position = new Vector3(newButton.transform.position.x + 150f * i, newButton.transform.position.y, newButton.transform.position.z);
            newButton.GetComponent<Image>().sprite = btn_Slot[i].ButtonSprite;


            //Debug.Log(newButton.transform.Find("Text").name);

            //���� ��ư�� ���� �׸� ã��(obj.text)
            GameObject buttonText = newButton.transform.Find("Text").gameObject;
            btn_Slot[i].CostText = buttonText.GetComponent<Text>();

            //ã�� text�� cost�� ����ȯ�ؼ� ������
            string conversion = btn_Slot[i]._cost.ToString();
            btn_Slot[i].CostText.text = conversion;


            //�ش� ��ư Ŭ���� ��������
            btn_Slot[index].ButtonObj.onClick.AddListener(() =>
            {

                TileButtonOnClick(index);

            });

        }


    }



    public void TileButtonOnClick(int num)
    {
        //���� ��ư ��ȣ�� 0�̸�(���̸�)
        if (num == 0)
        {
            //TileObjectBuilder.GetInstance.SetRoadTarget();
            if (Input.GetMouseButtonUp(0))
            {
                RoadBuilder.GetInstance.IsBuilding = true;
            }

        }
        else//�ش� Ÿ�� �ǹ� ����
        {
            TileObjectBuilder.GetInstance.SetBuildingTarget((Define.Building)num - 1);

        }

        _buildTileMenu.SetActive(false);
    }



}
