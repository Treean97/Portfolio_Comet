using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss_0 : Enemy
{
    [SerializeField]
    BossUI _BossUI;

    // ���� 1�� ���� ��ü
    [SerializeField]
    GameObject _BossOrb;

    bool _IsRunningPattern = false;

    [SerializeField]
    float _Pattern_0_MaxCoolTime = 30;
    [SerializeField]
    float _Pattern_0_CurCoolTime = 20;

    // ��ü�� ������ ��ġ
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

        // UI ����, �ʱ� �� ����
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

            DrainText(tDamage);

            // ü�¹� ����
            _BossUI.UpdateHpBarValue(_CurEnemyHP);

            return;
        }

        base.Damaged(tDamage);
                
        if (_CurEnemyHP <= 0)
        {
            _CurEnemyHP = Mathf.Clamp(_CurEnemyHP, 0, _MaxEnemyHP);
            Dead();
        }

        // ü�¹� ����
        _BossUI.UpdateHpBarValue(_CurEnemyHP);

    }

    public override void Dead()
    {
        _Animator.SetTrigger("Dead");

        base.Dead();

        // ����
        GameSceneUI tGameSceneUI = FindObjectOfType<GameSceneUI>();
        tGameSceneUI.EndingUI("����");
    }

    public void SetOrbPosition()
    {
        float tRadius = _OrbSpawnDistance;         //������
        float tAngle = _OrbSpawnAngle;             //���� ����

        Vector3 tTargetPos = _Player.transform.position + Vector3.up;

        float tCurrentAngle = 90;

        Vector3 tDirectionToTarget = (tTargetPos - transform.position).normalized;
        Quaternion tRotationToTarget = Quaternion.LookRotation(tDirectionToTarget);

        for (int i = 0; i < _OrbMaxCount; i++)
        {
            Vector3 tSpawnPos = new Vector3(
                Mathf.Cos(tCurrentAngle * Mathf.Deg2Rad) * tRadius,
                Mathf.Sin(tCurrentAngle * Mathf.Deg2Rad) * tRadius,
                0);

            tSpawnPos = tRotationToTarget * tSpawnPos + transform.position;

            _OrbPositions[i] = tSpawnPos;

            // ¦��
            if (i % 2 == 0)
            {
                tCurrentAngle += tAngle * (i + 1);
            }
            // Ȧ��
            else
            {
                tCurrentAngle -= tAngle * (i + 1);
            }
            
        }
    }

    public override void Bleeding(float tBleedingDamage, float tBleedingChance)
    {
        // �������̶�� ����
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

            // �ִ� ü���� n�ۼ�Ʈ
            Damaged((tBleedingDamage / 100) * _MaxEnemyHP);

            _CountBleedingTime++;

        }

        _IsBleeding = false;
    }


    // ���� ����
    // �ڷ�ƾ
    // 1. ��ü � ��ȯ�ؼ� �÷��̾� ���� ��ġ�� �ϳ��� �߻�
    IEnumerator BossPattern_0()
    {
        _Agent.isStopped = true;
        _IsRunningPattern = true;
        _AudioSource.clip = _BossPatternAudioClip[0];        

        GameObject[] tOrbs = new GameObject[_OrbMaxCount];

        SetOrbPosition();

        // ��ü ��ȯ
        for (int i = 0; i < _OrbMaxCount; i++)
        {
            _AudioSource.Play();

            tOrbs[i] = Instantiate<GameObject>(_BossOrb, _OrbPositions[i], Quaternion.identity);

            yield return new WaitForSeconds(_SpawnOrbDelay);
        }



        // ��ü �߻�        
        for (int i = 0; i < _OrbMaxCount; i++)
        {
            Vector3 tTargetPos = _Player.transform.position + Vector3.up;

            tOrbs[i].GetComponent<Boss_0_Orb>().SetTargetPosition(tTargetPos - _OrbPositions[i]);

            yield return new WaitForSeconds(_OrbShotDelay);
        }

        _Agent.isStopped = false;
        _IsRunningPattern = false;
    }

    public void Pattern_0_HitSount()
    {
        _AudioSource.clip = _BossPatternAudioClip[0];
        _AudioSource.Play();
    }


    // 2. ��� ���� �������� ������ �׸�ŭ ü�� ȸ��
    IEnumerator BossPattern_1()
    {
        // ����1 ���׸���� ����
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
        // ���׸��� �ǵ�����
        _SkinnedMeshRenderer.material = _Materials[0];
        _AudioSource.loop = false;
    }

}


