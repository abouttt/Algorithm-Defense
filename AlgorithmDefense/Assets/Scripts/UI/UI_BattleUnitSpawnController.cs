using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_BattleUnitSpawnController : MonoBehaviour
{
    //해당 버튼이 가지는 정보
    [System.Serializable]
    public class BuildButtonsInformation
    {
        public Sprite ButtonSprite; //버튼 이미지
        public int _many;           //명수
        [HideInInspector]
        public Text manyText;       //명수 텍스트
        [HideInInspector]
        public Button ButtonObj;   //생성된 버튼 오브젝트 정보
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


            //버튼 생성
            Button newButton = Instantiate(btn_Obj, container.transform);
            btn_Slot[i].ButtonObj = newButton;


            //생성된 버튼의 좌표와 이미지 변경
            newButton.transform.position = new Vector3(newButton.transform.position.x - 150f * i, newButton.transform.position.y, newButton.transform.position.z);
            newButton.GetComponent<Image>().sprite = btn_Slot[i].ButtonSprite;


            //Debug.Log(newButton.transform.Find("Text").name);

            //현재 버튼의 하위 항목 찾기(obj.text)
            GameObject buttonText = newButton.transform.Find("Text").gameObject;
            btn_Slot[i].manyText = buttonText.GetComponent<Text>();

            //찾은 text에 cost를 형변환해서 변경함
            string conversion = btn_Slot[i]._many.ToString();
            btn_Slot[i].manyText.text = conversion;


            //해당 버튼 클릭시 정보전달
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
