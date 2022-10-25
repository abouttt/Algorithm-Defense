using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_BattleUnitSpawnController : MonoBehaviour
{
    //�ش� ��ư�� ������ ����
    [System.Serializable]
    public class BuildButtonsInformation
    {
        public Sprite ButtonSprite; //��ư �̹���
        public int _many;           //���
        [HideInInspector]
        public Text manyText;       //��� �ؽ�Ʈ
        [HideInInspector]
        public Button ButtonObj;   //������ ��ư ������Ʈ ����
    }

    public BuildButtonsInformation[] BattleButtons;



    [SerializeField]
    private Button UnitButton;
    [SerializeField]
    private RectTransform BattleButtonContainer;


    private void Start()
    {

        CreateButton(BattleButtonContainer, UnitButton, BattleButtons);

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
            newButton.transform.position = new Vector3(newButton.transform.position.x - 150f * i, newButton.transform.position.y, newButton.transform.position.z);
            newButton.GetComponent<Image>().sprite = btn_Slot[i].ButtonSprite;


            //Debug.Log(newButton.transform.Find("Text").name);

            //���� ��ư�� ���� �׸� ã��(obj.text)
            GameObject buttonText = newButton.transform.Find("Text").gameObject;
            btn_Slot[i].manyText = buttonText.GetComponent<Text>();

            //ã�� text�� cost�� ����ȯ�ؼ� ������
            string conversion = btn_Slot[i]._many.ToString();
            btn_Slot[i].manyText.text = conversion;


            //�ش� ��ư Ŭ���� ��������
            btn_Slot[index].ButtonObj.onClick.AddListener(() =>
            {

                BattleButtonOnClick(index);
            });

        }


    }


    public void BattleButtonOnClick(int num)
    {
   

    }



}
