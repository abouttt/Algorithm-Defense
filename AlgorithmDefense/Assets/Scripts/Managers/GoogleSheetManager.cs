using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class GoogleSheetManager : MonoBehaviour
{
    public string URL = "https://docs.google.com/spreadsheets/d/10P8Dn319v-O0QbgBwSjkXi5HZtkVWst3ZPnSoBU2SDA";
    public string range;

    private const string _tsvFormat = "/export?format=tsv";

    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL + _tsvFormat + $"&range={range}");
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);
    }
}
