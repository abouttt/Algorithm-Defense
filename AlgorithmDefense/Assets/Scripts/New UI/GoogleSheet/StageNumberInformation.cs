using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StageNumberInformation : MonoBehaviour
{
    const string URL = "https://docs.google.com/spreadsheets/d/12832Ca3KkVRecaRPsPfUmrWzv9XDf8n5JllMFV44Pn4/export?format=tsv&range=A2:C";
    //const string URL = "https://script.google.com/macros/s/AKfycbxcyMz1T42QlZqBzBTiPHMYs-SJJlBFeqpGw6TN7Ws1_7EzS4SzGrL_lPuc0s8E0uBKUA/exec";

    // public InputField Star, Open;


    public InputField valueInput;

    //������ string���� �����ߵ�
    string num, star, open;

    public class StageAll
    {
        public string[] Stage;

    }


    [System.Serializable]
    public class GoogleData
    {
       
        public int StageNum;
        public int StageStar;
        public bool StageOpen;
        //public StageAll[] Stage;
        public  string[][] Stage;

    }

    internal readonly  GoogleData[] GD;


    //bool SetPass()
    //{
    //    //��������
    //    id = ID.text.Trim();
    //    pw = PW.text.Trim();

    //    if (id == "" || pw == "") return false;
    //    else return true;

    //}

    //���۽�Ʈ ���� ȣ��
    IEnumerator Start()
    {
        //�����͸� ����Ʈ �Ҷ� �����ִ� �Լ�
      // WWWForm form = new WWWForm();
        //form.AddField("value", "��");

        UnityWebRequest www =  UnityWebRequest.Get(URL);
       // UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

       
        if (www.isDone)
        {
            string data = www.downloadHandler.text;

            string[] _page1 = www.downloadHandler.text.Split('\n');

            string[] _page2;
            int count= _page1.Length;

            //GD = new GoogleData[count];

            for (int i = 0; i < _page1.Length; i++)
            {
                //Debug.Log("_page1[i]: "+_page1[i]);
                //�������� ����
                _page2 = _page1[i].Split('\t');

                for(int j = 0; j < 3; j++)
                {
                   // GD.Stage[i][j] = _page2[j];
                   Debug.Log("_page2[j]: " + _page2[j]);


                }


                GD[i].StageNum = int.Parse(_page2[0]);
                GD[i].StageStar = int.Parse(_page2[1]);
                if (_page2[2] == "TRUE")
                {
                    GD[i].StageOpen = true;

                }
                else if (_page2[2] == "FALSE")
                {
                    GD[i].StageOpen = false;
                }



            }

            for (int i=0;i<count;i++)
            {

          
                //GD.StageStar[i] = GD.Stage[i][i*3+1];
                //GD.StageOpen[i] = GD.Stage[i][i*3+2];

               // Debug.Log(GD.StageNum[i] + "�������� / ������: " + GD.StageStar[i] + " , ����: " + GD.StageOpen[i]);
              
                
            }

        }
        else
        {
            Debug.Log("���۽�Ʈ �������");
        }

      

    }


    //���� ��Ʈ �ȿ� ��ũ��Ʈ ȣ��

    //void Start()
    //{
    //    //num = "1";
    //    //star = "0";
    //    //open = "true";

    //    //WWWForm form = new WWWForm();

    //    ////������ ���� �̸��� ���������� �־���
    //    ////form.AddField("order", "register");
    //    ////form.AddField("order", "login");
    //    //form.AddField("order", "StartStageNumGet");

    //    //form.AddField("num", num);
    //    //form.AddField("star", star);
    //    //form.AddField("open", open);

    //    ////������
    //    //StartCoroutine(Post(form));


     
    //    //GetValue();

    //}



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
                //���
                string a = www.downloadHandler.text;

                Debug.Log(a);
                Response(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("�������");
            }

        }



    }

    public void Response(string json)
    {
        //�޾ƿ� ���� ����ִٸ�
        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        //�� ����
       // GD = JsonUtility.FromJson<GoogleData>(json);

        //if(GD.order== "getStartStageNum")
        //{

        //    for (int i=0;i<3;i++)
        //    {
        //        ss.num[0] = int.Parse(GD.num[i]);

        //        ss.star[i] = int.Parse(GD.star[i]);

        //        if(GD.open[i] == "TRUE")
        //        {
        //            ss.open[i] = true;

        //        }
        //        else if (GD.open[i] == "FALSE")
        //        {
        //            ss.open[i] = false;
        //        }
        //        else
        //        {
        //            Debug.Log(i + "�� open����");
        //        }
        //    }




        //}

        //Debug.Log("a:"+ss.num.Length);

        //for (int i = 0; i < ss.num.Length; i++)
        //{

        //    Debug.Log(ss.num[i] + "�������� / ������: " + ss.star[i] + " , ����: " + ss.open[i]);

        //}

        ////�����϶�
        //if (GD.result == "ERROR")
        //{
        //    Debug.Log(GD.order + "������� msg:" + GD.msg);
        //    return;
        //}

        //Debug.Log(GD.order + "����! msg:" + GD.msg);

        ////���� ��������
        //if (GD.order == "getValue")
        //{
        //    valueInput.text = GD.value;
        //}


    }



    //cache �� ����� ������ �������� ������
    public void SetValue()
    {

        WWWForm form = new WWWForm();

        form.AddField("order", "setValue");

        form.AddField("value", valueInput.text);

        //������
        StartCoroutine(Post(form));

    }





    public void GetValue()
    {

        WWWForm form = new WWWForm();

        form.AddField("order", "getStartStageNum");

        //������
        StartCoroutine(Post(form));

    }








}
