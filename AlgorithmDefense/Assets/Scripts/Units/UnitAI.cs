using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
    public Define.Move MoveType;
    public GameObject arrowPrefab;
    public Transform RotatePoint;
    public int Damage;

    private UnitManager _detectedUnit;
    private GameObject _tower;

    private Animator _anim;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int attackDamage;
    [SerializeField] private float _speed = 0f;
    [SerializeField] private float _range = 0f;
    private int rot;

    // Start is called before the first frame update
    void Start()
    {
        _anim = transform.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_detectedUnit && !_tower)
        {
            Move();
        }

        CheckLayer();
    }

    public void InflictDamage()
    {
        if (_detectedUnit)
        {
            bool unitDie = _detectedUnit.LoseHp(attackDamage);

            if (unitDie)
            {
                _anim.SetBool("Attack", false);

                _detectedUnit = null;
            }
        }
        else
        {
            if (_tower.gameObject.GetComponent<CastleGate>())
            {
                Managers.Game.CastleHP -= Damage;
            }
            else
            {
                Managers.Game.DungeonHP -= Damage;
            }
        }

    }

    public void ShootObject()
    {
        RotatePoint.rotation = Quaternion.Euler(0, 0, rot);
        Instantiate(arrowPrefab, transform.position, RotatePoint.rotation);

        if (_detectedUnit)
        {
            bool unitDie = _detectedUnit.LoseHp(attackDamage);

            if (unitDie)
            {
                _anim.SetBool("Attack", false);

                _detectedUnit = null;
            }
        }
        else
        {
            if (_tower.gameObject.GetComponent<CastleGate>())
            {
                Managers.Game.CastleHP -= Damage;
            }
            else
            {
                Managers.Game.DungeonHP -= Damage;
            }
        }
    }

    public void Move()
    {
        _anim.SetBool("Attack", false);

        if (MoveType == Define.Move.Down)
        {
            _anim.SetFloat("Hor", 0);
            _anim.SetFloat("Ver", -1);
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
            rot = -90;
        }
        else
        {
            _anim.SetFloat("Hor", 0);
            _anim.SetFloat("Ver", 1);
            transform.Translate(Vector2.up * _speed * Time.deltaTime);
            rot = 90;
        }
    }

    private void CheckLayer()
    {
        if (_detectedUnit)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _range, layerMask);

        foreach (Collider2D collider2D in colliders)
        {
            if (colliders != null)
            {
                if (collider2D.gameObject.layer == LayerMask.NameToLayer("Tower"))
                {
                    if (gameObject.layer == LayerMask.NameToLayer("Human"))
                    {
                        if (!collider2D.gameObject.GetComponent<MonsterGate>())
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!collider2D.gameObject.GetComponent<CastleGate>())
                        {
                            return;
                        }
                    }

                    _tower = collider2D.gameObject;
                }
                else
                {
                    _detectedUnit = collider2D.GetComponent<UnitManager>();
                }

                _anim.SetBool("Attack", true);
            }
        }
    }
}
