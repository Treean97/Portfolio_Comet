using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy_0 : Enemy
{
    [SerializeField]
    Animator _Animatior;

    [SerializeField]
    float _MaxDissolveAmount;



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

        _Material = _Renderer.material;

        ChangeState(STATE.Spawn);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        switch(_State)
        {
            case STATE.Spawn:
                break;

            case STATE.Movement:
                Movement();
                break;

            case STATE.Attack:
                Attack();
                break;

            case STATE.Dead:
                Dead();
                break;

        }

    }


    public override void Movement()
    {
        _Animatior.SetFloat("Speed", _CurSpeed);

        base.Movement();

        float tDis = Vector3.Distance(_Player.transform.position, transform.position);

        // 적 탐색 및 쿨타임 확인
        if (_AttackRange >= tDis && _CurAttackDelay >= _MaxAttackDelay)
        {
            ChangeState(STATE.Attack);
        }
    }

    public override void Attack()
    {
        base.Attack();

        // 애니메이션
        _Animatior.SetTrigger("Attack");

        // todo
        // 추후 애니메이션 끝나고 상태 변경으로 수정
        ChangeState(STATE.Movement);
    }

    public override void Dead()
    {
        if (!_IsDeadRoutineStart)
        {
            ChangeState(STATE.Dead);

            _IsDeadRoutineStart = true;

            StartCoroutine(DeadCoroutine());
        }      
    }

    IEnumerator DeadCoroutine()
    {
        base.Dead();

        // 콜라이더 Off
        gameObject.GetComponent<SphereCollider>().enabled = false;

        // 움직임 멈춤
        _Agent.isStopped = true;
        _Agent.velocity = Vector3.zero;
        _Agent.SetDestination(_Agent.gameObject.transform.position);

        // 애니메이션
        _Animatior.SetTrigger("Dead");

        // 애니메이션 길이 탐지로 생긴 버그인지 테스트
        //float tAnimTime = _Animatior.GetCurrentAnimatorStateInfo(0).normalizedTime;

        //// 죽는 모션 중
        //while (tAnimTime <= 1)
        //{
        //    yield return null;
        //}

        yield return new WaitForSeconds(1);

        // 종료 시 Dissolve
        // Dissolve & shadow off
        _Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        _Renderer.receiveShadows = false;

        float _CurDissolveAmount = 0;

        while (_CurDissolveAmount <= _MaxDissolveAmount)
        {
            _CurDissolveAmount += Time.deltaTime;

            _Material.SetFloat("_DissolveAmount", _CurDissolveAmount);

            yield return null;
        }

        // 오브젝트 반납
        ObjectPool._Inst.ReturnObject(this.gameObject);
    }
}
