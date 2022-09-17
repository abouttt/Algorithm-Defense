using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour
{
    private static MouseController s_instance;
    public static MouseController GetInstance { get { Init(); return s_instance; } }

    public Vector3 MouseWorldPos { get; private set; }
    public Vector3Int MouseCellPos { get; private set; }

    private Camera _camera;

    private void Start()
    {
        Init();

        _camera = Camera.main;
    }

    private void Update()
    {
        MouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        MouseCellPos = Managers.Tile.GetGrid().WorldToCell(MouseWorldPos);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (TileObjectBuilder.GetInstance.IsBuilding)
            {
                return;
            }

            var go = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(MouseCellPos);
            if (go)
            {
                var building = go.GetComponent<BaseBuilding>();
                if (building.HasUI)
                {
                    //������ Ŭ������ �ǹ� ���� ã��
                    var name = building.name.Replace("(Clone)", String.Empty);
                    Debug.Log("�����̸�: " + name);

                    //�ϵ� �����ִ� �ش� �ǹ�UI �ݱ�(���� �̸��� �ǹ� Ŭ�� ���� ����)
                    UI_BuildingMenager.GetInstance.CloseUIController();

                    //Ŭ���� �ǹ� �̸��� ������Ʈ ����
                    UI_BuildingMenager.GetInstance.ShowUIController((Define.Building)Enum.Parse(typeof(Define.Building), name), building);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (TileObjectBuilder.GetInstance.IsBuilding)
            {
                return;
            }

            // �ǹ� ����.
            var building = Managers.Tile.GetTile(Define.Tilemap.Building, MouseCellPos);
            if (building)
            {
                Managers.Tile.SetTile(Define.Tilemap.Building, MouseCellPos, null);
            }

            // �� �׷� ����.
            var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(MouseCellPos);
            var road = go.GetComponent<Road>();
            if (road)
            {
                foreach (var pos in Road.RoadGroupDic[road.GroupNumber])
                {
                    Managers.Tile.SetTile(Define.Tilemap.Road, pos, null);
                }

                Road.RoadGroupDic.Remove(road.GroupNumber);
            }
        }
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            var go = GameObject.Find("@MouseController");
            if (!go)
            {
                go = Util.CreateGameObject<MouseController>("@MouseController");
            }

            s_instance = go.GetComponent<MouseController>();
        }
    }
}
