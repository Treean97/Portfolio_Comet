using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour
{
    [SerializeField]
    public EnemyStatus _EnemyStatus;

    [SerializeField]
    protected NavMeshAgent _Agent;

    [SerializeField]
    protected Player _Player;

    [SerializeField]
    protected float _MaxEnemyHP;

    [SerializeField]
    protected float _CurEnemyHP;

    [SerializeField]
    protected float _StopBleedingTime = 10;

    [SerializeField]
    protected float _CountBleedingTime = 0;

    [SerializeField]
    protected bool _IsBleeding = false;

    [SerializeField]
    protected float _AttackRange;

    [SerializeField]
    protected float _AttackPower;

    [SerializeField]
    protected float _MaxAttackDelay;

    [SerializeField]
    protected float _CurAttackDelay;

    [SerializeField]
    protected Rigidbody _Rigidbody;

    [SerializeField]
    protected Renderer _Renderer;

    protected Material _Material;

    protected bool _IsDeadRoutineStart = false;

    Vector3 _LastPos;
    protected float _CurSpeed;

    [SerializeField]
    protected AudioSource _AudioSource;

    // 공격 0 죽음 1
    [SerializeField]
    AudioClip[] _AudioClip;

    [SerializeField]
    DamageFontMgr _DamageFontMgr;
    

    public enum STATE
    {
        Spawn = 0,
        Movement,
        Attack,
        Dead
    }

    public STATE _State;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _Player = FindObjectOfType<Player>();
        transform.forward = _Player.transform.position - transform.position;
        _LastPos = transform.position;
        _DamageFontMgr = GameObject.FindGameObjectWithTag("DamageFontMgr").GetComponent<DamageFontMgr>();
    }

    private void OnEnable()
    {
        if(_Player == null)
        {
            _Player = FindObjectOfType<Player>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_CurAttackDelay <= _MaxAttackDelay)
        {
            _CurAttackDelay += Time.deltaTime;
        }

        _CurSpeed = CalculateSpeed();
    }


    public void ChangeState(STATE tState)
    {
        _State = tState;
    }

    public virtual void Movement()
    {
        _Agent.destination = _Player.transform.position;
    }

    public virtual void Attack()
    {
        if(_CurAttackDelay >= _MaxAttackDelay)
        {
            // 플레이어를 바라보게
            transform.LookAt(_Player.transform.position);

            _AudioSource.clip = _AudioClip[0];
            _AudioSource.Play();

            // Player 스크립트에서 상속받는 형태로 바꾸고 데미지 받는 함수 추가
            _Player.Damaged(_AttackPower);

            _CurAttackDelay = 0;
        }
    }

    public virtual void Damaged(float tDamage)
    {
        _CurEnemyHP -= tDamage;
        DamageText(tDamage);

        if (_CurEnemyHP <= 0)
        {
            Dead();

            StopCoroutine("BleedingCoroutine");
        }
    }

    public virtual void Damaged()
    {

    }

    protected virtual void DamageText(float tDamage)
    {
        _DamageFontMgr.DamageUIObjectPool(tDamage, this.transform.position);
    }

    public virtual void Bleeding(float tBleedingDamage, float tBleedingChance)
    {
        // 출혈중이라면 갱신
        if (_IsBleeding)
        {
            _CountBleedingTime = 0;
        }
        else
        {
            StartCoroutine(BleedingCoroutine(tBleedingDamage));
        }
    }

    protected virtual IEnumerator BleedingCoroutine(float tBleedingDamage)
    {
        _IsBleeding = true;

        while (_CountBleedingTime < _StopBleedingTime)
        {
            yield return new WaitForSeconds(1f);

            // 최대 체력의 n퍼센트
            Damaged((tBleedingDamage / 100) * _MaxEnemyHP);

            _CountBleedingTime++;

        }

        _IsBleeding = false;
    }


    public virtual void Dead()
    {
        int tRandom = Random.Range(0, 100);
        _Player.UpdateMoney(tRandom);

        _AudioSource.clip = _AudioClip[1];
        _AudioSource.Play();

        // 출혈 멈춤
        _CountBleedingTime = _StopBleedingTime;
    }

    protected float CalculateSpeed()
    {
        Vector3 tCurPos = transform.position;
        float tDistance = Vector3.Distance(tCurPos, _LastPos);
        float speed = tDistance / Time.deltaTime;
        _LastPos = tCurPos;
        return speed;
    }

}
