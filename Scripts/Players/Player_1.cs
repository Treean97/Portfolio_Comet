using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1 : Player
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

    bool _Skill_0_IsActive = false;

    float _Skill_0_CurCoolTime = 20;
    float _Skill_0_MaxCoolTime = 20;
    float _Skill_0_MaxDurationTime = 0;

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

        // skill_0
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_Skill_0_CurCoolTime >= _Skill_0_MaxCoolTime)
            {
                Skill_0_Start();
            }
            else
            {
                _GameSceneUI.WarningTextUI("Skill is CoolTime");
            }
        }

        if (_Skill_0_CurCoolTime <= _Skill_0_MaxCoolTime)
        {
            _Skill_0_CurCoolTime += Time.deltaTime;
        }

    }

    private void LateUpdate()
    {
        if (Time.timeScale != 0)
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
        _Skill_0_CurCoolTime = 0;

        // 효과
        _Skill_0_IsActive = true;

        // 쿨타임, 지속 시간 UI
        _SkillMgr.Skill(_Skill_0_MaxCoolTime, _Skill_0_MaxDurationTime, 0);

        // 효과 켜짐 애니메이션

    }

    public override void Damaged(float tDamage)
    {
        if(_Skill_0_IsActive)
        {
            _Skill_0_IsActive = false;

            // 지속 시간 UI 끄기
            _SkillMgr.SkillDurationTimeInfinityOff(0);

            // 효과 꺼짐 애니메이션

        }
        else
        {
            base.Damaged(tDamage);
        }        
    }
}
