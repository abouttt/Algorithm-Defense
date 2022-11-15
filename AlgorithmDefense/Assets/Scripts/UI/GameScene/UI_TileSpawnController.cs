using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_TileSpawnController : MonoBehaviour
{

    private static UI_TileSpawnController s_instance;
    public static UI_TileSpawnController GetInstance { get { Init(); return s_instance; } }

    //�ش� ��ư�� ������ ����
    [System.Serializable]
    public class BuildButtonsInformation
    {
        public Sprite ButtonSprite; //��ư �̹���
        public int _cost;           //����
        [HideInInspector]
        public TextMeshProUGUI CostText;       //���� �ؽ�Ʈ
        [HideInInspector]
        public Button ButtonObj;   //������ ��ư ������Ʈ ����
        public bool anima;
    }

    public BuildButtonsInformation[] BuildButtons;


    [SerializeField]
    private Button TileButton;
    [SerializeField]
    private RectTransform buildButtonContainer;
    private CallSkill _CallSkill;
    private GameObject _buildTileMenu;


    private void Start()
    {

        //�ش� ������Ʈ�� �θ� ã�Ƽ� ã��
        _buildTileMenu = GameObject.Find("BuildSpawnButtons");
        _CallSkill = FindObjectOfType<CallSkill>();
        CreateButton(buildButtonContainer, TileButton, BuildButtons);
        GoldChange();
    }


    public void GoldChange()
    {
        for (int i = 0; i < BuildButtons.Length; i++)
        {

            int firstGoid = Managers.Game.Gold;


            if (firstGoid >= BuildButtons[i]._cost)
            {
                BuildButtons[i].ButtonObj.GetComponent<Image>().color = new Color(1f, 1f, 1f);
                if (BuildButtons[i].anima)
                {
                    BuildButtons[i].ButtonObj.transform.DOScale(1.2f, 0f);
                    BuildButtons[i].ButtonObj.transform.DOScale(1f, 0.8f).SetEase(Ease.OutBounce);
                    BuildButtons[i].anima = false;
                }
            }
            else
            {
                BuildButtons[i].ButtonObj.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                BuildButtons[i].anima = true;
            }

        }
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
            newButton.transform.localPosition = new Vector3(newButton.transform.localPosition.x + 170f * i, newButton.transform.localPosition.y, newButton.transform.localPosition.z);
            newButton.GetComponent<Image>().sprite = btn_Slot[i].ButtonSprite;


            //Debug.Log(newButton.transform.Find("Text").name);

            //���� ��ư�� ���� �׸� ã��(obj.text)
            GameObject buttonText = newButton.transform.Find("Text").gameObject;
            btn_Slot[i].CostText = buttonText.GetComponent<TextMeshProUGUI>();

            //ã�� text�� cost�� ����ȯ�ؼ� ������
            string conversion = btn_Slot[i]._cost.ToString();
            btn_Slot[i].CostText.text = conversion;


            btn_Slot[i].ButtonObj.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            btn_Slot[i].anima = true;

           //�ش� ��ư Ŭ���� ��������
           btn_Slot[index].ButtonObj.onClick.AddListener(() =>
            {

                //TileButtonOnClick(index);

                int firstGoid = Managers.Game.Gold;
                if (_CallSkill._isSpawning1==false && _CallSkill._isSpawning2 == false && _CallSkill._isSpawning3 == false)
                {
                    GoldAnimation.GetInstance.GoldExpenditure(btn_Slot[index]._cost);

                    if (firstGoid >= btn_Slot[index]._cost)
                    {
                        btn_Slot[index].ButtonObj.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                        //Debug.Log("��ų����");
                        _CallSkill.skill(index);
                    }
                    else
                    {
                        //��� ���� ����
                        UI_NoticeTextSet.GetInstance.LackOfGold();
                        Managers.Sound.Play("UI/Failed_To_Use_Skill", Define.Sound.Effect);
                    }
                }
                else
                {
                    //��ų ��Ÿ�� ����
                    UI_NoticeTextSet.GetInstance.SkillCooldown();
                    Managers.Sound.Play("UI/Failed_To_Use_Skill", Define.Sound.Effect);
                }

            });

        }


    }


    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("BuildSpawnButtons");
            if (!go)
            {
                go = new GameObject { name = "BuildSpawnButtons" };
                go.AddComponent<UI_TileSpawnController>();
            }

            s_instance = go.GetComponent<UI_TileSpawnController>();
        }
    }





}
