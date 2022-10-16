using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class StageTileInformation : MonoBehaviour
{
    //kgw sheet
    //const string URL = "https://script.google.com/macros/s/AKfycbxcyMz1T42QlZqBzBTiPHMYs-SJJlBFeqpGw6TN7Ws1_7EzS4SzGrL_lPuc0s8E0uBKUA/exec";

    const string URL = "https://script.google.com/macros/s/AKfycbxzoHfRTzsaIR5un1lkT9sm0Tiwhm0iIhEWepodlykJAy0db_Oaj4BAlDPt_aSslDFN/exec";




    private int stageNum;

    [System.Serializable]
    public class GoogleData
    {
        public string order;
        public string result;
        public string deta;
        public string value;

    }
    public GoogleData GD;

    public int[][] StageTileData = new int[5][];

    [HideInInspector]
    public int StarCount;   //Ŭ���� �� ���� �� ����


    private void Start()
    {
        StarCount = 0;

        //Ŭ���� �������� ��ȣ ��������
        stageNum = PlayerPrefs.GetInt("Num");
        Debug.Log(stageNum);
        //stageNum = 1;

        WWWForm form = new WWWForm();

        //������ ���� �̸��� ���������� �־���
        form.AddField("order", "getStageTile");
        form.AddField("num", stageNum);

        //������
        StartCoroutine(Post(form));



    }


    IEnumerator Post(WWWForm form)
    {

        //using�� �Ⱦ��� ����� ���� ���� ����
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            //������ ���� ���
            yield return www.SendWebRequest();

            //�� ��������
            if (www.isDone)
            {
                //�� ���� �� msg���
                Response(www.downloadHandler.text);

                switch (GD.order)
                {
                    case "getStageTile":
                        SetTileData();

                        //StarCount = 3;
                        //GameClearSetStarCount();

                        //�ʵ���� �ҷ�����

                        var tile = Managers.Resource.Load<TileBase>($"{Define.BUILDING_TILE_PATH}{Define.Building.Gateway}");
                        for (int y = 0; y < 5; y++)
                        {
                            for (int x = 0; x < 5; x++)
                            {
                                if (StageTileData[y][x] == -1)
                                {
                                    continue;
                                }

                                tile = Managers.Resource.Load<TileBase>($"{Define.BUILDING_TILE_PATH}{(Define.Building)StageTileData[y][x]}");
                                Managers.Tile.SetTile(
                                    Define.Tilemap.Building,
                                    new Vector3Int(
                                        Managers.Game.Setting.StartPosition.x + x + 1,
                                        Managers.Game.Setting.StartPosition.y + y + 1, 0), 
                                        tile);
                            }
                        }

                        break;

                    case "setValue":
                        break;

                }




               



            }
            else
            {
                Debug.Log("Google Apps Script �������.");
            }


        }

    }


    //�� ���� �� msg���
    public void Response(string json)
    {
        //�޾ƿ� ���� ����ִٸ�
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("�󰪹���");
            return;
        }

        //�� ����
        GD = JsonUtility.FromJson<GoogleData>(json);

        //�����϶�
        if (GD.result == "ERROR")
        {
            Debug.Log("(" + GD.order + "): " + GD.result + " [deta: " + GD.deta + "]");
            return;
        }


        Debug.Log("(" + GD.order + "): " + GD.result + " [deta: " + GD.deta + "]");
    }

    public void SetTileData()
    {
        string[] data = GD.deta.Split(',');
        int count = 0;

        Debug.Log("data.Length: " + data.Length);

        for (int i = 0; i < 5; i++)
        {
            StageTileData[i] = new int[5];

            for (int j = 0; j < 5; j++)
            {
                StageTileData[i][j] = int.Parse(data[count]);
                count++;
            }
        }

    }



    //Ŭ����� �� ���� �� ����é�� ����
    public void GameClearSetStarCount()
    {
        //Ŭ��� ���Ѱ� �ƴ϶��
        if (StarCount != 0)
        {
            WWWForm form = new WWWForm();

            //������ ���� �̸��� ���������� �־���
            form.AddField("order", "setValue");
            form.AddField("star", StarCount);

            //������
            StartCoroutine(Post(form));
        }
    }

    //�ڷΰ���
    public void BackStartScene()
    {
        //����ȭ������ �̵�
        SceneManager.LoadScene(0);
    }

    //�ٽ��ϱ�
    public void NowStageAgain()
    {
        //Game��(1��)�ٽ� ����
        SceneManager.LoadScene(1);
    }

    //����é��
   public void NextStageNumSet()
    {

        int stage = PlayerPrefs.GetInt("StageCount");
        if (stage != stageNum)
        {
            //���� �������� ��ȣ�� ��������
            PlayerPrefs.SetInt("Num", stageNum + 1);

            //Game��(1��)�ٽ� ����
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("��� �������� Ŭ����!!!");
            //Start��(0��) ����
            SceneManager.LoadScene(0);
        }

    }





}
