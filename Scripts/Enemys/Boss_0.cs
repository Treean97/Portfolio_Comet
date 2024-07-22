using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0 : Enemy
{
    [SerializeField]
    BossUI _BossUI;

    // 패턴 1에 사용될 구체
    [SerializeField]
    GameObject _BossOrb;

    bool _IsRunningPattern = false;

    [SerializeField]
    float _Pattern_0_MaxCoolTime = 30;
    [SerializeField]
    float _Pattern_0_CurCoolTime = 20;

    // 구체가 생성될 위치
    [SerializeField]
    Vector3[] _OrbPositions;

    float _OrbSpawnDistance = 5f;

    float _OrbSpawnAngle = 20f;

    int _OrbMaxCount = 5;

    float _SpawnOrbDelay = 1;

    float _OrbShotDelay = 1;


    bool _IsDrain = false;

    [SerializeField]
    float _Pattern_1_MaxCoolTime = 60f;
    [SerializeField]
    float _Pattern_1_CurCoolTime = 0f;

    float _MaxDrainTime = 5;
    float _CurDrainTime = 0;

    [SerializeField]
    Material[] _Materials;

    [SerializeField]
    SkinnedMeshRenderer _SkinnedMeshRenderer;

    [SerializeField]
    Animator _Animator;

    [SerializeField]
    AudioClip[] _BossPatternAudioClip;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _MaxEnemyHP = _EnemyStatus.GetEnemyHP;
        _CurEnemyHP = _MaxEnemyHP;
        _Agent.stoppingDistance = _EnemyStatus.GetEnemyAttackRange;
        _Agent.speed = _EnemyStatus.GetEnemySpeed;
        _AttackRange = _EnemyStatus.GetEnemyAttackRange;
        _AttackPower = _EnemyStatus.GetEnemyAttackPower;
        _MaxAttackDelay = _EnemyStatus.GetEnemyAttackDelay;

        // UI 연결, 초기 값 설정
        _BossUI = FindObjectOfType<BossUI>();
        _BossUI.SetHpBarMaxValue(_MaxEnemyHP);
        _BossUI.UpdateHpBarValue(_MaxEnemyHP);

        _OrbPositions = new Vector3[_OrbMaxCount];
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();

        if (Vector3.Distance(this.gameObject.transform.position, base._Player.transform.position) <= _AttackRange)
        {
            if(_MaxAttackDelay <= _CurAttackDelay)
            {
                Attack();
            }
            
        }
        else
        {
            Movement();
        }

        if(_Pattern_0_MaxCoolTime >= _Pattern_0_CurCoolTime)
        {
            _Pattern_0_CurCoolTime += Time.deltaTime;
        }
        else if(!_IsRunningPattern)
        {
            _Pattern_0_CurCoolTime = 0;
            StartCoroutine(BossPattern_0());
        }

        if (_Pattern_1_MaxCoolTime >= _Pattern_1_CurCoolTime)
        {
            _Pattern_1_CurCoolTime += Time.deltaTime;
        }
        else if(!_IsRunningPattern)
        {
            _Pattern_1_CurCoolTime = 0;
            StartCoroutine(BossPattern_1());
        }

    }

    public override void Movement()
    {
        _Animator.SetFloat("Speed", _CurSpeed);

        base.Movement();
    }

    public override void Attack()
    {
        _Animator.SetTrigger("Attack");

        base.Attack();
    }

    public override void Damaged(float tDamage)
    {
        if(_IsDrain)
        {
            _CurEnemyHP += tDamage;
            _CurEnemyHP = Mathf.Clamp(_CurEnemyHP, 0, _MaxEnemyHP);

            // 체력바 갱신
            _BossUI.UpdateHpBarValue(_CurEnemyHP);

            return;
        }

        base.Damaged(tDamage);

        if (_CurEnemyHP <= 0)
        {
            _CurEnemyHP = Mathf.Clamp(_CurEnemyHP, 0, _MaxEnemyHP);
            Dead();
        }

        // 체력바 갱신
        _BossUI.UpdateHpBarValue(_CurEnemyHP);

    }

    public override void Dead()
    {
        _Animator.SetTrigger("Dead");

        base.Dead();

        // 엔딩
        GameSceneUI tGameSceneUI = FindObjectOfType<GameSceneUI>();
        tGameSceneUI.EndingUI("성공");
    }

    public void SetOrbPosition()
    {
        float radius = _OrbSpawnDistance;         //반지름
        float angle = _OrbSpawnAngle;             //간격 각도

        float currentAngle = 90;

        for (int i = 0; i < _OrbMaxCount; i++)
        {
            _OrbPositions[i] =
                new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius, Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius, 0)
                + this.transform.position;

            // 짝수
            if(i % 2 == 0)
            {
                currentAngle += angle * (i + 1);
            }
            // 홀수
            else
            {
                currentAngle -= angle * (i + 1);
            }
            
        }
    }

    public override void Bleeding(float tBleedingDamage, float tBleedingChance)
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

    protected override IEnumerator BleedingCoroutine(float tBleedingDamage)
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


    // 보스 패턴
    // 코루틴
    // 1. 구체 몇개 소환해서 플레이어 현재 위치에 하나씩 발사
    IEnumerator BossPattern_0()
    {
        _Agent.isStopped = true;
        _IsRunningPattern = true;
        _AudioSource.clip = _BossPatternAudioClip[0];        

        GameObject[] tOrbs = new GameObject[_OrbMaxCount];

        SetOrbPosition();

        // 구체 소환
        for (int i = 0; i < _OrbMaxCount; i++)
        {
            _AudioSource.Play();

            tOrbs[i] = Instantiate<GameObject>(_BossOrb, _OrbPositions[i], Quaternion.identity);

            yield return new WaitForSeconds(_SpawnOrbDelay);
        }

        // 구체 발사        
        for (int i = 0; i < _OrbMaxCount; i++)
        {
            tOrbs[i].GetComponent<Boss_0_Orb>().SetTargetPosition(_Player.transform.position - _OrbPositions[i]);

            yield return new WaitForSeconds(_OrbShotDelay);
        }

        _Agent.isStopped = false;
        _IsRunningPattern = false;
    }



    // 2. 잠시 동안 데미지를 받으면 그만큼 체력 회복
    IEnumerator BossPattern_1()
    {
        // 패턴1 머테리얼로 변경
        _SkinnedMeshRenderer.material = _Materials[1];
        _AudioSource.clip = _BossPatternAudioClip[1];
        _AudioSource.Play();
        _AudioSource.loop = true;

        _CurDrainTime = 0;

        _Agent.isStopped = true;
        _IsRunningPattern = true;
        _IsDrain = true;

        while(_MaxDrainTime >= _CurDrainTime)
        {
            yield return null;
            _CurDrainTime += Time.deltaTime;
        }


        _IsDrain = false;
        _Agent.isStopped = false;
        _IsRunningPattern = false;
        // 머테리얼 되돌리기
        _SkinnedMeshRenderer.material = _Materials[0];
        _AudioSource.loop = false;
    }

}


