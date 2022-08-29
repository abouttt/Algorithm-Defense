using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CitizenController : BaseController
{
    private Animator Anim; // �׽�Ʈ �ڵ�
    private SpriteRenderer spriteX;// �׽�Ʈ �ڵ�
    public CitizenData Data = new CitizenData();

    public void Awake() // �׽�Ʈ �ڵ�
    {
        Anim = this.transform.GetComponent<Animator>(); // �׽�Ʈ �ڵ�
        spriteX = this.transform.GetComponent<SpriteRenderer>(); // �׽�Ʈ �ڵ�
    }
    public override void Init()
    {
        _state = Define.State.Moving;
    }

    public void SetNextDestination()
    {
        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        switch (Data.MoveType)
        {
            case Define.Move.Down:
                cellPos.y--;
                break;
            case Define.Move.Up:
                cellPos.y++;
                break;
            case Define.Move.Right:
                cellPos.x++;
                //transform.localScale = new Vector3(1, 1, 1);
                spriteX.flipX = false; // �׽�Ʈ �ڵ�
                break;
            case Define.Move.Left:
                cellPos.x--;
                //transform.localScale = new Vector3(-1, 1, 1);
                spriteX.flipX = true; // �׽�Ʈ �ڵ�
                break;
        }

        Data.Destination = Managers.Tile.GetCellCenterToWorld(Define.Tilemap.Ground, cellPos);
    }

    public void SetReverseMoveType()
    {
        switch (Data.MoveType)
        {
            case Define.Move.Right:
                Data.MoveType = Define.Move.Left;
                break;
            case Define.Move.Left:
                Data.MoveType = Define.Move.Right;
                break;
            case Define.Move.Up:
                Data.MoveType = Define.Move.Down;
                break;
            case Define.Move.Down:
                Data.MoveType = Define.Move.Up;
                break;
        }
    }

    protected override void UpdateMoving()
    {
        Anim.SetInteger("AnimState", 1); // �׽�Ʈ �ڵ�
        transform.position = Vector2.MoveTowards(transform.position, Data.Destination, (Data.MoveSpeed * Time.deltaTime));

        var cellPos = Managers.Tile.GetWorldToCell(Define.Tilemap.Ground, transform.position);
        if (Vector2.Distance(transform.position, Data.Destination) <= 0.01f)
        {
            CheckRoad(cellPos);
            SetNextDestination();
        }

        CheckBuilding(cellPos);
    }

    private void CheckBuilding(Vector3Int cellPos)
    {
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
                SetReverseMoveType();
                break;
            case Define.Road.TU:
            case Define.Road.TD:
                if (IsMoveTypeUD())
                {
                    SetReverseMoveType();
                }
                break;
            case Define.Road.TL:
            case Define.Road.TR:
                if (!IsMoveTypeUD())
                {
                    SetReverseMoveType();
                }
                break;
            case Define.Road.CUL:
                Data.MoveType = IsMoveTypeUD() ? Define.Move.Left : Define.Move.Down;
                break;
            case Define.Road.CUR:
                Data.MoveType = IsMoveTypeUD() ? Define.Move.Right : Define.Move.Down;
                break;
            case Define.Road.CDR:
                Data.MoveType = IsMoveTypeUD() ? Define.Move.Right : Define.Move.Up;
                break;
            case Define.Road.CDL:
                Data.MoveType = IsMoveTypeUD() ? Define.Move.Left : Define.Move.Up;
                break;
        }
    }

    private bool IsMoveTypeUD() => (Data.MoveType == Define.Move.Up) || (Data.MoveType == Define.Move.Down);
}
