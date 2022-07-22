using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenController : BaseController
{
    [field: SerializeField]
    public Define.Citizen CitizenType { get; private set; }

    public Define.Move MoveType = Define.Move.None;

    [SerializeField]
    private float _moveSpeed;
    private SpriteRenderer _weaponSpriteRenderer;
    private Vector3 _dest;

    public override void Init()
    {
        _weaponSpriteRenderer = Util.FindChild<Transform>(gameObject, "R_Weapon", recursive: true).GetComponent<SpriteRenderer>();
        _state = Define.State.Moving;
    }
    
    public void SetWeapon(Sprite weaponSprite) => _weaponSpriteRenderer.sprite = weaponSprite;

    public void SetNextDestination()
    {
        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        switch (MoveType)
        {
            case Define.Move.Down:
                cellPos.y--;
                break;
            case Define.Move.Up:
                cellPos.y++;
                break;
            case Define.Move.Right:
                cellPos.x++;
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case Define.Move.Left:
                cellPos.x--;
                transform.localScale = new Vector3(1, 1, 1);
                break;
        }

        _dest = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
    }

    public void SetOppositeMoveType()
    {
        switch (MoveType)
        {
            case Define.Move.Right:
                MoveType = Define.Move.Left;
                break;
            case Define.Move.Left:
                MoveType = Define.Move.Right;
                break;
            case Define.Move.Up:
                MoveType = Define.Move.Down;
                break;
            case Define.Move.Down:
                MoveType = Define.Move.Up;
                break;
        }
    }

    protected override void UpdateMoving()
    {
        transform.position = Vector2.MoveTowards(transform.position, _dest, (_moveSpeed * Time.deltaTime));

        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        if (Vector2.Distance(transform.position, _dest) <= 0.01f)
        {
            CheckRoad(cellPos);
            SetNextDestination();
        }

        CheckBuilding(cellPos);
    }

    private void CheckBuilding(Vector3Int cellPos)
    {
        //var tile = Managers.Tile.GetTile(Define.Tilemap.Building, cellPos) as Tile;
        var go = Managers.Tile.GetTilemap(Define.Tilemap.Building).GetInstantiatedObject(cellPos);
        if (!go)
        {
            return;
        }

        go.GetComponent<BaseBuilding>().EnterTheBuilding(this);
    }

    private void CheckRoad(Vector3Int cellPos)
    {
        var go = Managers.Tile.GetTilemap(Define.Tilemap.Road).GetInstantiatedObject(cellPos);
        if (!go)
        {
            return;
        }

        var road = go.GetComponent<Road>();
        switch (road.RoadType)
        {
            case Define.Road.B:
            case Define.Road.BU:
            case Define.Road.BD:
            case Define.Road.BL:
            case Define.Road.BR:
                SetOppositeMoveType();
                break;
            case Define.Road.TU:
            case Define.Road.TD:
                if (IsMoveTypeUD())
                {
                    SetOppositeMoveType();
                }
                break;
            case Define.Road.TL:
            case Define.Road.TR:
                if (!IsMoveTypeUD())
                {
                    SetOppositeMoveType();
                }
                break;
            case Define.Road.CUL:
                MoveType = IsMoveTypeUD() ? Define.Move.Left : Define.Move.Down;
                break;
            case Define.Road.CUR:
                MoveType = IsMoveTypeUD() ? Define.Move.Right : Define.Move.Down;
                break;
            case Define.Road.CDR:
                MoveType = IsMoveTypeUD() ? Define.Move.Right : Define.Move.Up;
                break;
            case Define.Road.CDL:
                MoveType = IsMoveTypeUD() ? Define.Move.Left : Define.Move.Up;
                break;
        }
    }

    private bool IsMoveTypeUD() => (MoveType == Define.Move.Up) || (MoveType == Define.Move.Down);
}
