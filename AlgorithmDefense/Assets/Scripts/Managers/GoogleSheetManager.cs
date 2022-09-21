using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class GoogleSheetManager : MonoBehaviour
{
    const string URL = "https://docs.google.com/spreadsheets/d/1rbYCu0s6tjQM9YmjNNmWJRriqRHzgoTzIZzXo8Nj_Uw/export?format=tsv";
    const string building = "https://docs.google.com/spreadsheets/d/1rbYCu0s6tjQM9YmjNNmWJRriqRHzgoTzIZzXo8Nj_Uw/export?format=tsv&range=B2:B4";
    const string posX = "https://docs.google.com/spreadsheets/d/1rbYCu0s6tjQM9YmjNNmWJRriqRHzgoTzIZzXo8Nj_Uw/export?format=tsv&range=C2:C4";
    const string posY = "https://docs.google.com/spreadsheets/d/1rbYCu0s6tjQM9YmjNNmWJRriqRHzgoTzIZzXo8Nj_Uw/export?format=tsv&range=D2:D4";

    List<string> _buildings = new();
    List<Vector3Int> _pos = new();

    bool _isBuildingLoad = false;
    bool _isPosLoad = false;

    private void Start()
    {
        StartCoroutine(GetBuildingList());
        StartCoroutine(GetPosList());
    }

    private void Update()
    {
        if (_isBuildingLoad && _isPosLoad)
        {
            TileBase tile = null;
            for (int i = 0; i < _buildings.Count(); i++)
            {
                tile = Managers.Resource.Load<Tile>($"{Define.BUILDING_TILE_PATH}{_buildings[i]}");
                Managers.Tile.SetTile(Define.Tilemap.Building, _pos[i], tile);
            }

            _isBuildingLoad = false;
            _isPosLoad = false;
        }
    }

    public IEnumerator GetBuildingList()
    {
        UnityWebRequest www = UnityWebRequest.Get(building);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        _buildings = data.Split("\r\n").ToList();
        _isBuildingLoad = true;
    }

    public IEnumerator GetPosList()
    {
        UnityWebRequest xw = UnityWebRequest.Get(posX);
        yield return xw.SendWebRequest();

        UnityWebRequest yw = UnityWebRequest.Get(posY);
        yield return yw.SendWebRequest();

        string x = xw.downloadHandler.text;
        string y = yw.downloadHandler.text;

        var xs = x.Split("\r\n");
        var ys = y.Split("\r\n");

        for (int i = 0; i < xs.Length; i++)
        {
            _pos.Add(new Vector3Int(int.Parse(xs[i]), int.Parse(ys[i]), 0));
        }

        _isPosLoad = true;
    }
}
