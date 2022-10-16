using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Text;
using System.Linq;


public class StageNumberInformation : MonoBehaviour
{
    //kgw sheet
    //const string URL = "https://docs.google.com/spreadsheets/d/12832Ca3KkVRecaRPsPfUmrWzv9XDf8n5JllMFV44Pn4/export?format=tsv&range=A2:C";

    const string URL = "https://docs.google.com/spreadsheets/d/1rbYCu0s6tjQM9YmjNNmWJRriqRHzgoTzIZzXo8Nj_Uw/export?format=tsv&range=A2:C";


    [HideInInspector]
    public string[][] StageStringData;
    [HideInInspector]
    public int[] StageNum;
    [HideInInspector]
    public int[] StageStar;
    [HideInInspector]
    public bool[] StageOpen;

    public void Awake()
    {
        StartCoroutine("GetStageData");
    }


    //���۽�Ʈ ���� ȣ��
    IEnumerator GetStageData()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        //�����͸� �� ��������
        if (www.isDone)
        {

            string[] data = www.downloadHandler.text.Split('\n');

            StageStringData = new string[data.Length][];

            StageNum = new int[data.Length];
            StageStar = new int[data.Length];
            StageOpen = new bool[data.Length];

            UI_StartSceneUIAnimation.GetInstance.StageUIBar = new UI_StartSceneUIAnimation.StageUIBarInformation[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                //�������� ����
                StageStringData[i] = data[i].Split('\t');
            }

            SetStageData(data.Length);

        }
        else
        {
            Debug.Log("Google Sheet �������");
        }



    }


    public void SetStageData(int length)
    {
        Debug.Log("GoogleSheetDataSet");

        for (int i = 0; i < length; i++)
        {
            StageNum[i] = int.Parse(StageStringData[i][0]);
            StageStar[i] = int.Parse(StageStringData[i][1]);
            StageOpen[i] = bool.Parse(StageStringData[i][2]);

            UI_StartSceneUIAnimation.GetInstance.StageUIBar[i] = new UI_StartSceneUIAnimation.StageUIBarInformation();

            UI_StartSceneUIAnimation.GetInstance.StageUIBar[i].stageNum = StageNum[i];
            UI_StartSceneUIAnimation.GetInstance.StageUIBar[i].starCount = StageStar[i];
            UI_StartSceneUIAnimation.GetInstance.StageUIBar[i].open = StageOpen[i];

        }

        //stage�޴� �����ϱ� ȣ��
        UI_StartSceneUIAnimation.GetInstance.SetStage();
    }


}
