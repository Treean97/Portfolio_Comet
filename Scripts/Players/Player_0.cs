using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_0 : Player
{
    [SerializeField]
    Animator _PlayerAnimator;

    float _SpineRotationAngle = 0;
    float _SpineMaxRotationAngle = 30f;
    float _SpineAngle = 60;

    [SerializeField]
    Transform _PlayerSpine;

    [SerializeField]
    float _SpineRotSpeed;

    float _MoveAniSpeed = 1.5f;

    // 스킬 속도 보너스%
    float _Skill_0_SpeedBonus = 50;

    float _Skill_0_MaxCoolTime = 10;
    float _Skill_0_CurCoolTime = 10;
    float _Skill_0_MaxDurationTime = 5;

    void Start()
    {
        // 캐릭터 스탯 가져오기
        // 체력 UI 세팅
        //_TotalHp = _PlayerStatus.GetPlayerHP;
        //_CurHp = _TotalHp;
        //_PlayerUI.SetHpBarMaxValue(_TotalHp);
        //_PlayerUI.UpdateHpBarValue(_TotalHp);

        //_TotalSpeed = _PlayerStatus.GetPlayerSpeed;
        //_TotalJumpCount = _PlayerStatus.GetPlayerMaxJumpCount;
        //_TotalJumpPower = _PlayerStatus.GetPlayerJumpPower;
        //_PlayerCriChance = _PlayerStatus.GetPlayerCriChange;
        //_TotalAttackDelay = _PlayerStatus.GetPlayerAttackDelay;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_Skill_0_CurCoolTime >= _Skill_0_MaxCoolTime)
            {
                StartCoroutine(Skill_0());
            }
            else
            {
                _GameSceneUI.WarningTextUI("Skill is CoolTime");
            }
        }       

        if(_Skill_0_CurCoolTime <= _Skill_0_MaxCoolTime)
        {
            _Skill_0_CurCoolTime += Time.deltaTime;
        }

    }

    private void LateUpdate()
    {
        if(Time.timeScale != 0)
        {
            float tMouseY = Input.GetAxisRaw("Mouse Y") * _SpineRotSpeed;

            _SpineRotationAngle = Mathf.Clamp(_SpineRotationAngle - tMouseY, -_SpineMaxRotationAngle, _SpineMaxRotationAngle);
            _PlayerSpine.localRotation = Quaternion.Euler(_SpineRotationAngle, _SpineAngle, 0);
        }        
    }

    protected override void PlayerMovement()
    {
        base.PlayerMovement();

        // 좌, 우 애니메이션
        _PlayerAnimator.SetFloat("MoveX", _MoveDir.x);

        // 앞, 뒤 애니메이션
        _PlayerAnimator.SetFloat("MoveZ", _MoveDir.z);
    }

    protected override void PlayerJump()
    {
        base.PlayerJump();

        _PlayerAnimator.SetTrigger("Jump");
    }

    protected override void PlayerFire()
    {
        base.PlayerFire();

        _PlayerAnimator.SetTrigger("Fire");
    }

    void Skill_0_Start()
    {
        _IsRunning = true;

        _Skill_0_CurCoolTime = 0;

        //_SkillMgr.Skill_0_Duration(_Skill_0_MaxCoolTime, _Skill_0_MaxDurationTime);
        _SkillMgr.Skill(_Skill_0_MaxCoolTime, _Skill_0_MaxDurationTime, 0);

        // 이동속도 50% 증가
        CalApplySpeedMul(_Skill_0_SpeedBonus);

        // 애니메이션 속도 증가
        _PlayerAnimator.SetFloat("MoveAniSpeed", _MoveAniSpeed);

    }

    void Skill_0_End()
    {
        _IsRunning = false;

        // 이동속도 50% 다시 원래대로
        CalApplySpeedMul(-_Skill_0_SpeedBonus);

        // 애니메이션 속도 원래대로
        _PlayerAnimator.SetFloat("MoveAniSpeed", 1);
    }

    IEnumerator Skill_0()
    {
        float tSkill_0_CurDurationTime = 0;

        Skill_0_Start();

        while (_Skill_0_MaxDurationTime >= tSkill_0_CurDurationTime)
        {
            tSkill_0_CurDurationTime += Time.deltaTime;

            yield return null;
        }

        Skill_0_End();
    }
}
