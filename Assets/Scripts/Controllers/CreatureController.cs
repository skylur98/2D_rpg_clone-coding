using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CreatureController : MonoBehaviour
{
    public float _speed = 5.0f;


    protected Vector3Int _cellPos = Vector3Int.zero;
    protected Animator _animator;
    protected SpriteRenderer _sprite;

    CreatureState _state = CreatureState.Idle;
    public CreatureState State
    {
        get { return State; }
        set
        {
            if (_state == value)
                return;

            _state = value;
        }
    }
    MoveDir _lastDir = MoveDir.None;
    MoveDir _dir = MoveDir.Down;
    public MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_dir == value)
                return;

            switch (value)
            {
                case MoveDir.Up:
                    _animator.Play("WALK_BACK");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
                case MoveDir.Down:
                    _animator.Play("WALK_FRONT");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
                case MoveDir.Left:
                    _animator.Play("WALK_RIGHT");
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    break;
                case MoveDir.Right:
                    _animator.Play("WALK_RIGHT");
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    break;             
            }

            _dir = value;
            if (value != MoveDir.None)
                _lastDir = value;
        }
    }

    public virtual void UpdateAnimation()
    {
        if(_state == CreatureState.Idle)
        {
            switch (_lastDir)
            {
                case MoveDir.Up:
                _animator.Play("IDLE_BACK");
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
                case MoveDir.Down:
                _animator.Play("IDLE_Front");
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
                case MoveDir.Left:
                _animator.Play("IDLE_Right");
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                break;
                case MoveDir.Right:
                _animator.Play("IDLE_Right");
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;    
            }
        }
        if (_state == CreatureState.Moving)
        {

        }
        if (_state == CreatureState.Skill)
        {

        }
        else
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateController();

    }

    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f);
        transform.position = pos;
    }

    protected virtual void UpdateController()
    {
        UpdatePosition();
        UpdateIsMoving();
    }

    void UpdatePosition()
    {
        if (State != CreatureState.Moving)
            return;

        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(_cellPos) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - transform.position;

        float dist = moveDir.magnitude;
        if (dist < _speed * Time.deltaTime)
        {
            transform.position = destPos;
            State = CreatureState.Idle;
        }
        else
        {
            transform.position += moveDir.normalized * _speed * Time.deltaTime;
            State = CreatureState.Moving;
        }
    }

    void UpdateIsMoving()
    {
        if (State == CreatureState.Idle && _dir != MoveDir.None)
        {
            Vector3Int destPos = _cellPos;
            switch (_dir)
            {
                case MoveDir.Up:
                    destPos += Vector3Int.up;
                    break;
                case MoveDir.Down:
                    destPos += Vector3Int.down;
                    break;
                case MoveDir.Left:
                    destPos += Vector3Int.left;
                    break;
                case MoveDir.Right:
                    destPos += Vector3Int.right;
                    break;
            }

            if (Managers.Map.CanGo(destPos))
            {
                _cellPos = destPos;
                State = CreatureState.Moving;
            }
        }
    }

}
