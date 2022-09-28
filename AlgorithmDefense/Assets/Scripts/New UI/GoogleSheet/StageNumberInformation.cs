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

    //정보는 string으로 보내야됨
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
    //    //공백제거
    //    id = ID.text.Trim();
    //    pw = PW.text.Trim();

    //    if (id == "" || pw == "") return false;
    //    else return true;

    //}

    //구글시트 직접 호출
    IEnumerator Start()
    {
        //데이터를 포스트 할때 도와주는 함수
      // WWWForm form = new WWWForm();
        //form.AddField("value", "값");

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
                //스테이지 구분
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

               // Debug.Log(GD.StageNum[i] + "스테이지 / 별갯수: " + GD.StageStar[i] + " , 오픈: " + GD.StageOpen[i]);
              
                
            }

        }
        else
        {
            Debug.Log("구글시트 응답없음");
        }

      

    }


    //구글 시트 안에 스크립트 호출

    //void Start()
    //{
    //    //num = "1";
    //    //star = "0";
    //    //open = "true";

    //    //WWWForm form = new WWWForm();

    //    ////각각의 정보 이름과 넣을정보를 넣어줌
    //    ////form.AddField("order", "register");
    //    ////form.AddField("order", "login");
    //    //form.AddField("order", "StartStageNumGet");

    //    //form.AddField("num", num);
    //    //form.AddField("star", star);
    //    //form.AddField("open", open);

    //    ////보내기
    //    //StartCoroutine(Post(form));


     
    //    //GetValue();

    //}



    IEnumerator Post(WWWForm form)
    {
        //using을 안쓰면 통신을 안할 때가 있음
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            //보내는 동안 대기
            yield return www.SendWebRequest();

            //다 보냈으면
            if (www.isDone)
            {
                //출력
                string a = www.downloadHandler.text;

                Debug.Log(a);
                Response(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("응답없음");
            }

        }



    }

    public void Response(string json)
    {
        //받아온 값이 비어있다면
        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        //값 저장
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
        //            Debug.Log(i + "번 open오류");
        //        }
        //    }




        //}

        //Debug.Log("a:"+ss.num.Length);

        //for (int i = 0; i < ss.num.Length; i++)
        //{

        //    Debug.Log(ss.num[i] + "스테이지 / 별갯수: " + ss.star[i] + " , 오픈: " + ss.open[i]);

        //}

        ////에러일때
        //if (GD.result == "ERROR")
        //{
        //    Debug.Log(GD.order + "응답없음 msg:" + GD.msg);
        //    return;
        //}

        //Debug.Log(GD.order + "실행! msg:" + GD.msg);

        ////값을 가져오면
        //if (GD.order == "getValue")
        //{
        //    valueInput.text = GD.value;
        //}


    }



    //cache 에 저장된 데이터 기준으로 가져옴
    public void SetValue()
    {

        WWWForm form = new WWWForm();

        form.AddField("order", "setValue");

        form.AddField("value", valueInput.text);

        //보내기
        StartCoroutine(Post(form));

    }





    public void GetValue()
    {

        WWWForm form = new WWWForm();

        form.AddField("order", "getStartStageNum");

        //보내기
        StartCoroutine(Post(form));

    }








}
