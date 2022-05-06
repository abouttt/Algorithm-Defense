using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CitizenController : BaseController
{
    [field: SerializeField]
    public Define.Citizen CitizenType { get; private set; } = Define.Citizen.None;
    public Define.MoveType MoveType { get; set; } = Define.MoveType.None;
    [field: SerializeField]
    public bool IsExit { get; set; } = false;

    [SerializeField]
    private float _moveSpeed = 0.0f;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Citizen;
        _state = Define.State.Moving;
    }

    protected override void UpdateMoving()
    {
        var nextPos = new Vector3();

        switch (MoveType)
        {
            case Define.MoveType.Right:
                nextPos = (Vector3.right * 2) + Vector3.down;
                break;
            case Define.MoveType.Left:
                nextPos = (Vector3.left * 2) + Vector3.up;
                break;
            case Define.MoveType.Up:
                nextPos = Vector3.up + (Vector3.right * 2);
                break;
            case Define.MoveType.Down:
                nextPos = Vector3.down + (Vector3.left * 2);
                break;
        }

        transform.position += nextPos * (_moveSpeed * Time.deltaTime);
    }
}
