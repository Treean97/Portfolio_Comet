using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Player;

public class Player : MonoBehaviour
{
    // 플레이어 상태
    public enum GunState
    {
        CanFire,
        CanNotFire
    }
    public GunState _GunState;

  
    protected enum PlayerState
    {
        Idle,
        Walk,
        Jump
    }

    [SerializeField]
    protected PlayerState _PlayerState;

    [SerializeField]
    LineRenderer _BulletLineRenderer;

    [SerializeField]
    float CheckGroundRaycastDistance;

    [SerializeField]
    bool _IsGround = false;

    // 플레이어 스테이터스
    [SerializeField]
    public PlayerStatus _PlayerStatus;

    // 플레이어 총알 스테이터스
    [SerializeField]
    public BulletStatus _PlayerBullet_Normal_Status;

    // 플레이어 치명타 총알 스테이터스
    [SerializeField]
    public BulletStatus _PlayerBullet_Cri_Status;

    [SerializeField]
    Camera _MainCamera;

    [SerializeField]
    GameSceneMgr _GameSceneMgr;

    [SerializeField]
    protected CharacterController _CController;

    // Raycast 지점
    [SerializeField]
    Transform _RaycastPos;

    // 발사 지점
    [SerializeField]
    Transform _FirePos;

    // 미사일 발사 지점
    [SerializeField]
    Transform _MissilePos;

    [SerializeField]
    protected GameSceneUI _GameSceneUI;

    [SerializeField]
    LayerMask _ItemLayerMask;

    int _IgnoreLayerMask;

    [SerializeField]
    protected PlayerUI _PlayerUI;

    [SerializeField]
    ItemMgr _ItemMgr;

    [SerializeField]
    GameObject _Missile;

    // 움직임
    [SerializeField]
    protected float _Gravity;

    protected Vector3 _MoveDir;

    protected float _y;

    // 플레이어 스테이터스
    [SerializeField]
    protected float _TotalSpeed;

    protected float _ApplySpeed;

    [SerializeField]
    protected  float _TotalJumpPower;

    [SerializeField]
    protected float _TotalJumpCount;

    [SerializeField]
    protected float _CurJumpCount;

    [SerializeField]
    protected float _DamageMultiply = 100;

    [SerializeField]
    protected float _TotalHp;

    [SerializeField]
    protected float _CurHp;

    [SerializeField]
    protected float _PlayerCriChance;

    [SerializeField]
    protected float _TotalAttackDelay;

    [SerializeField]
    protected float _CurAttackDelay;

    [SerializeField]
    DamageFontMgr _DamageFontMgr;

    // 인벤토리
    [SerializeField]
    Dictionary<int, int> _PlayerInventory = new Dictionary<int, int>();

    public Dictionary<int, int> GetPlayerInventory { get { return _PlayerInventory; } }

    // 아이템 보유 체크
    [SerializeField]
    bool _Item100_Active = false;

    [SerializeField]
    bool _Item101_Active = false;

    // 아이템 효과
    [SerializeField]
    float _BleedingDamage = 0;

    [SerializeField]
    float _BleedingChance = 0;

    [SerializeField]
    float _MissileChance = 0;

    [SerializeField]
    float _ItemCheckRaycastRange = 0;

    [SerializeField]
    int _Money = 0;

    [SerializeField]
    protected SkillMgr _SkillMgr;

    // 총 탄퍼짐
    float _MaxSpread = 10;

    [SerializeField]
    float _CurSpread = 0;
    float _SpreadDegree = 3f;
    float _RestoreSpread = 2.5f;

    [SerializeField]
    GameSounds _GameSounds;

    [SerializeField]
    protected bool _IsRunning = false;

    protected float _CurStepSoundCoolTime = 0;

    [SerializeField]
    protected float _MaxStepSoundCoolTime;


    // 플레이어 정보 설정
    void Awake()
    {
        _PlayerStatus = GameManager._Inst._PlayerStatus;
        _MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _SkillMgr = GameObject.FindGameObjectWithTag("SkillMgr").GetComponent<SkillMgr>();
        _GameSceneMgr = GameObject.FindGameObjectWithTag("GameSceneMgr").GetComponent<GameSceneMgr>();
        _GameSceneUI = GameObject.FindGameObjectWithTag("GameSceneUI").GetComponent<GameSceneUI>();
        _PlayerUI = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>();
        _ItemMgr = GameObject.FindGameObjectWithTag("ItemMgr").GetComponent<ItemMgr>();
        _DamageFontMgr = GameObject.FindGameObjectWithTag("DamageFontMgr").GetComponent<DamageFontMgr>();

        _TotalHp = _PlayerStatus.GetPlayerHP;
        _CurHp = _TotalHp;
        _PlayerUI.SetHpBarMaxValue(_TotalHp);
        _PlayerUI.UpdateHpBarValue(_TotalHp);

        _TotalSpeed = _PlayerStatus.GetPlayerSpeed;
        _ApplySpeed = _TotalSpeed;
        _TotalJumpCount = _PlayerStatus.GetPlayerMaxJumpCount;
        _TotalJumpPower = _PlayerStatus.GetPlayerJumpPower;
        _PlayerCriChance = _PlayerStatus.GetPlayerCriChange;
        _TotalAttackDelay = _PlayerStatus.GetPlayerAttackDelay;

        _IgnoreLayerMask = (-1) - (1 << LayerMask.NameToLayer("ItemLayer"));

        _BulletLineRenderer = GetComponent<LineRenderer>();

        // 사용할 점을 두개로 변경
        _BulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        _BulletLineRenderer.enabled = false;

        // 사운드 매니저
        _GameSounds = GameObject.FindObjectOfType<GameSounds>();    

    }

    // Start is called before the first frame update
    void Start()
    {
        _GunState = GunState.CanFire;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // 돈 테스트
        if(Input.GetKeyDown(KeyCode.P))
        {
            UpdateMoney(1000);
        }

        // 인벤토리 확인 테스트
        if(Input.GetKeyDown(KeyCode.I))
        {
            foreach (KeyValuePair<int, int> kvp in _PlayerInventory)
            {
                Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
            }
        }

        // 아이템 테스트
        if(Input.GetKeyDown(KeyCode.M))
        {
            ObjectPool._Inst.GetObject("Missile");
        }


        GroundCheckRay();

        if (_IsGround)
        {
            _CurJumpCount = 0;

            // 점프가 끝난 후 착지 했다면
            if(_PlayerState == PlayerState.Jump)
            {
                ChangeState(PlayerState.Idle);

                // 사운드
                _GameSounds.JumpEndSounds();
            }
        }
        else
        {
            _y -= _Gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _CurJumpCount < _TotalJumpCount)
        {
            PlayerJump();            
        }

        PlayerMovement();

        if (Input.GetMouseButton(0) )
        {
            if(_CurAttackDelay >= _TotalAttackDelay && _GunState == GunState.CanFire)
            {
                PlayerFire();
            }

            BulletSpread();
            
        }
        else
        {
            RestoreSpread();
        }

        if (_CurAttackDelay <= _TotalAttackDelay)
        {
            _CurAttackDelay += Time.deltaTime;
        }

        if(_CurStepSoundCoolTime <= _MaxStepSoundCoolTime)
        {
            _CurStepSoundCoolTime += Time.deltaTime;
        }

        CheckItemRaycast();

        
    }

    void GroundCheckRay()
    {
        if(Physics.Raycast(transform.position, Vector3.down, CheckGroundRaycastDistance))
        {
            _IsGround = true;
        }
        else
        {
            _IsGround = false;
        }

    }

    void BulletSpread()
    {
        if(_CurSpread <= _MaxSpread)
        {
            _CurSpread += _SpreadDegree * Time.deltaTime;
        }
        
    }

    void RestoreSpread()
    {
        if(_CurSpread >= 0)
        {
            _CurSpread -= _RestoreSpread * Time.deltaTime;
        }        
    }

    protected virtual void PlayerMovement()
    {
        // 입력값
        float tHorizonal = Input.GetAxisRaw("Horizontal");
        float tVertical = Input.GetAxisRaw("Vertical");

        // 방향 지정 및 정규화
        _MoveDir = new Vector3(tHorizonal, 0, tVertical);
        _MoveDir = _MoveDir.normalized;

        // 점프 중이거나 달리는 중이 아닐 때
        if(_PlayerState != PlayerState.Jump)
        {
            // 방향 입력이 있으면 (움직이는 중)
            if (_MoveDir.magnitude != 0)
            {
                // 달리는 중이 아니라면
                if (!_IsRunning)
                {
                    ChangeState(PlayerState.Walk);
                }

                if (_CurStepSoundCoolTime >= _MaxStepSoundCoolTime)
                {
                    _CurStepSoundCoolTime = 0;

                    if(_IsRunning)
                    {
                        _GameSounds.RunSounds();
                    }
                    else
                    {
                        _GameSounds.WalkSounds();
                    }

                }
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }
        }       


        // 변환
        Vector3 _MoveWorldDir = this.transform.TransformDirection(_MoveDir);

        // y값(점프 or 중력) 지정
        _MoveWorldDir.y = _y;

        // 움직임
        _CController.Move(_MoveWorldDir * _ApplySpeed * Time.deltaTime);

        // 카메라와 전방을 맞춤
        Vector3 lookForward = new Vector3(_MainCamera.transform.forward.x, 0f, _MainCamera.transform.forward.z).normalized;
        this.transform.forward = lookForward;
    }

    /* %형태로 입력(ex 50% = 50, -50% = -50) */
    protected virtual void CalApplySpeedMul(float tVar = 1)
    {        
        _ApplySpeed += _TotalSpeed * (tVar * 0.01f);
    }

    /* +형태로 입력(ex +50 = 50, -50 = -50)*/
    protected virtual void CalApplySpeedPlus(float tVar = 0)
    {
        _ApplySpeed += _TotalSpeed + tVar;
    }

    protected virtual void PlayerJump()
    {
        ChangeState(PlayerState.Jump);
        // 사운드
        _GameSounds.JumpStartSounds();

        // y축 점프값 입력
        _y = _PlayerStatus.GetPlayerJumpPower + _TotalJumpPower;

        // 점프 카운트 증가
        _CurJumpCount++;
    }

    protected virtual void CheckItemRaycast()
    {
        if (Physics.Raycast(_RaycastPos.position, transform.forward, out RaycastHit tHit, _ItemCheckRaycastRange, _ItemLayerMask))
        {
            // 대상이 아이템이면
            if (tHit.collider.CompareTag("Item"))
            {
                // 아이템 UI On, 다른 UI가 켜져있다면 Off
                if (_GameSceneUI._ItemUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._ItemUIGO);
                }

                // UI정보갱신
                Items tItem = tHit.collider.GetComponent<Items>();
                _GameSceneUI.UpdateItemUI(tItem._ItemStatus);

                // 아이템 획득
                if (Input.GetKeyDown(KeyCode.E))
                {
                    AddItem(tItem._ItemStatus);

                    // 아이템 설명 UI 끄기
                    _GameSceneUI.HideItemUI();

                    tItem.OnDestory();
                }
            }

            // 대상이 선택형 아이템이면
            else if(tHit.collider.CompareTag("SelectItem"))
            {
                if(_GameSceneUI._SelectItemText.gameObject.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._SelectItemText.gameObject);
                }

                // 아이템 구매 텍스트 UI On
                if (_GameSceneUI._ItemBuyUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._SelectItemText.gameObject);
                }

                // 일시정지 상태가 아니면 획득 가능
                if (Input.GetKeyDown(KeyCode.E) && _GameSceneUI._UIState != GameSceneUI.UIState.Running)
                {
                    _GameSceneUI.SelectItemUIOnOff();                                       

                    // 아이템 제거
                    Destroy(tHit.collider.gameObject);
                }
            }

            // 대상이 아이템 생성기면
            else if (tHit.collider.CompareTag("ItemSpawner"))
            {
                // 필요 재화 계산
                int tNeedMoney = _GameSceneMgr._BuyCount * 10;

                // 아이템 구매 UI 갱신
                _GameSceneUI.UpdateBuyItemUI(tNeedMoney);

                // 아이템 구매 UI On
                if (_GameSceneUI._ItemBuyUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._ItemBuyUIGO);
                }

                // 일시정지 상태가 아니면 구매 가능
                if (Input.GetKeyDown(KeyCode.E) && _GameSceneUI._UIState != GameSceneUI.UIState.Running)
                {
                    // 재화가 충분하면
                    if (_Money >= tNeedMoney)
                    {
                        // 생성기 전방에 아이템 생성
                        _ItemMgr.SpawnItem(tHit.transform.parent.gameObject);

                        // BuyCount증가
                        _GameSceneMgr._BuyCount++;

                        // 재화 차감
                        UpdateMoney(-tNeedMoney);
                    }
                    // 재화부족UI
                    else
                    {
                        _GameSceneUI.WarningTextUI("Not Enough Money");
                    }

                }
            }

            // 대상이 드론이면
            else if (tHit.collider.CompareTag("Drone"))
            {
                // 필요 재화 계산
                int tNeedMoney = _GameSceneMgr._DronePrice;

                // 아이템 구매 UI 갱신
                _GameSceneUI.UpdateBuyItemUI(tNeedMoney);

                // 아이템 구매 UI On
                if (_GameSceneUI._ItemBuyUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._ItemBuyUIGO);
                }

                // 일시정지 상태가 아니면 구매 가능
                if (Input.GetKeyDown(KeyCode.E) && _GameSceneUI._UIState != GameSceneUI.UIState.Running)
                {
                    // 재화가 충분하면
                    if (_Money >= tNeedMoney)
                    {
                        // 드론 작동
                        Drone tDrone = tHit.collider.GetComponentInParent<Drone>();
                        tDrone.ActiveDrone();

                        // 재화 차감
                        UpdateMoney(-tNeedMoney);
                    }
                    // 재화부족UI
                    else
                    {
                        _GameSceneUI.WarningTextUI("Not Enough Money");
                    }

                }
            }

            // 대상이 보스 생성기면
            else if(tHit.collider.CompareTag("BossSpawner"))
            {
                // UI On
                if (_GameSceneUI._BossSpawnUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._BossSpawnUIGO);
                }

                // 일시정지 상태가 아니면 보스 생성 가능
                if (Input.GetKeyDown(KeyCode.E) && _GameSceneUI._UIState != GameSceneUI.UIState.Running)
                {
                    // 보스 소환
                    tHit.collider.GetComponentInParent<BossSpanwer>().SpawnBoss();
                }
            }
        }
        else
        {
            _GameSceneUI.HideBuySpawnerUI();
            _GameSceneUI.HideItemUI();
            _GameSceneUI.HideSelectItemText();
            _GameSceneUI.HideBossSpawnUI();
        }
    }

    protected virtual void PlayerFire()
    {
        // 사운드
        _GameSounds.GunSounds();

        _CurAttackDelay = 0;

        //Vector3 tRayDir = _MainCamera.ScreenPointToRay(Input.mousePosition).direction;

        //Ray tRay = new Ray(_FirePos.position, tRayDir);

        //RaycastHit tHit;

        //if (Physics.Raycast(tRay, out tHit, Mathf.Infinity, _IgnoreLayerMask))
        //{
        //    // 방향
        //    var tDir = tHit.point - _FirePos.transform.position;

        //    var tBullets = ObjectPoolMgr.GetObject(); // Instantiate(bulletPrefab, transform.position + direction.normalized, Quaternion.identity).GetComponent<Bullet>();
        //    tBullets.transform.position = _FirePos.position + tDir.normalized;

        //    tBullets.DamageCal(_DamageMultiply);

        //    // 효과 체크
        //    CheckPassiveItemIsActive(tBullets);

        //    tBullets.Shoot(tDir.normalized);

        //}

        // LineRenderer버전
        // 탄퍼짐 오차

        // Vector3 tTarget = _MainCamera.ScreenPointToRay(Input.mousePosition).direction;
        var tRay = _MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        RaycastHit tHit;

        Vector3 tAimPoint = Vector3.zero;

        if (Physics.Raycast(tRay, out tHit, Mathf.Infinity, _IgnoreLayerMask))
        {
            tAimPoint = tHit.point;
        }

        var tFixTarget = tAimPoint - _FirePos.position;

        // 상하 오차
        float tRandomSpread = Random.Range(- _CurSpread / 2, _CurSpread / 2);
        tFixTarget = Quaternion.AngleAxis(tRandomSpread, Vector3.up) * tFixTarget;

        // 좌우 오차
        tRandomSpread = Random.Range(-_CurSpread / 2, _CurSpread / 2);
        tFixTarget = Quaternion.AngleAxis(tRandomSpread, Vector3.right) * tFixTarget;


        if (Physics.Raycast(_FirePos.transform.position, tFixTarget, out tHit, Mathf.Infinity, _IgnoreLayerMask))
        {
            float tDamage = 0;

            // 치명타 판단
            // 치명타 O
            if (IsCritical())
            {
                tDamage = _PlayerBullet_Cri_Status.GetBulletPower;
            }
            else
            {
                tDamage = _PlayerBullet_Normal_Status.GetBulletPower;
            }

            // 최종 데미지 계산
            tDamage *= (_DamageMultiply / 100);

            // 적이면
            if(tHit.transform.CompareTag("Enemy"))
            {
                // 데미지 주기
                Enemy tEnemy = tHit.transform.GetComponent<Enemy>();

                tEnemy.Damaged(tDamage);

                //_DamageFontMgr.DamageUICheack(tDamage, tHit.point);
                // _DamageFontMgr.DamageUIObjectPool(tDamage, tHit.point);

                // 패시브 계산
                CheckPassiveItemIsActive(tEnemy);
            }

            // 라인 랜더러
            StartCoroutine(FireCoroutine(tHit.point));

        }
    }

    IEnumerator FireCoroutine(Vector3 tHitPos)
    {
        // 선의 시작점은 총구의 위치
        _BulletLineRenderer.SetPosition(0, _FirePos.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        _BulletLineRenderer.SetPosition(1, tHitPos);
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        _BulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        _BulletLineRenderer.enabled = false;
    }

    // 치명타 계산
    bool IsCritical()
    {
        // 치명타 O
        if (Random.Range(0, 100) <= _PlayerCriChance)
        {
            return true;
        }
        // 치명타 X
        else
        {
            return false;
        }        
    }

    // 아이템 획득
    public virtual void AddItem(ItemStatus tItemStatus)
    {
        // 인벤토리에 아이템 확인
        // 있으면
        if (_PlayerInventory.ContainsKey(tItemStatus.GetItemId))
        {
            // 갯수만 1개 추가
            _PlayerInventory[tItemStatus.GetItemId] += 1;
        }
        // 없으면
        else
        {
            // 아이템id, 갯수 1개 추가
            _PlayerInventory.Add(tItemStatus.GetItemId, 1);
        }

        UpdateStatus(tItemStatus);
    }

    // 아이템을 통한 Status 수정
    protected virtual void UpdateStatus(ItemStatus tItemStatus)
    {
        // 아이템 타입 확인
        switch ((int)tItemStatus.GetItemType)
        {
            // Status 변경 아이템
            case 0:
                CheckStatusItem(tItemStatus);
                break;

            // Passive 아이템
            case 1:
                CheckPassiveItem(tItemStatus);
                break;

        }
    }

    protected virtual void CheckStatusItem(ItemStatus tItemStatus)
    {
        switch ((int)tItemStatus.GetItemAdvType)
        {
            // DamageMultiply
            case 0:
                // Add
                if ((int)tItemStatus.GetItemAdvCalType == 0)
                {
                    _DamageMultiply += tItemStatus.GetItemAdvValue;
                }
                // Multiply
                else if ((int)tItemStatus.GetItemAdvCalType == 1)
                {
                    _DamageMultiply *= 1 + (tItemStatus.GetItemAdvValue / 100);
                }

                break;

            // MaxJumpCount
            case 1:
                _TotalJumpCount += tItemStatus.GetItemAdvValue;
                break;

            // JumpPower
            case 2:
                _TotalJumpPower += tItemStatus.GetItemAdvValue;
                break;

            // Speed
            case 3:
                _TotalSpeed += tItemStatus.GetItemAdvValue;
                // 실제 속도도 변경
                CalApplySpeedPlus(tItemStatus.GetItemAdvValue);
                break;

            // Critical
            case 4:
                _PlayerCriChance += tItemStatus.GetItemAdvValue;
                break;

            // None
            default:
                break;
        }
    }

    // Passive 타입 아이템 체크
    protected virtual void CheckPassiveItem(ItemStatus tItemStatus)
    {
        switch (tItemStatus.GetItemId)
        {
            case 100:
                // 최초 획득 시 출혈 효과 활성화
                if (_PlayerInventory.ContainsKey(tItemStatus.GetItemId))
                {
                    _Item100_Active = true;
                }

                // 출혈 확률 증가
                _BleedingChance += tItemStatus._GetPassiveChance;

                // 확률 상한 제한
                _BleedingChance = Mathf.Clamp(_BleedingChance, 0, 100);

                // 출혈 데미지 증가
                _BleedingDamage += tItemStatus.GetItemAdvValue;

                break;

            case 101:
                // 최초 획득 시 미사일 발사 효과 활성화
                if (_PlayerInventory.ContainsKey(tItemStatus.GetItemId))
                {
                    _Item101_Active = true;
                }

                // 발사 확률 추가
                _MissileChance += tItemStatus._GetPassiveChance;

                // 확률 상한 제한
                _MissileChance = Mathf.Clamp(_MissileChance, 0, 100);
                break;
        }

    }

    // 아이템 활성화 체크
    protected virtual void CheckPassiveItemIsActive(Enemy tEnemy)
    {
        //if (_Item100_Active)
        //{
        //    tBullet.IsActiveBleeding = true;
        //    tBullet._BleedingDamage = this._BleedingDamage;
        //    tBullet._BleedingChance = this._BleedingChance;
        //}

        //if (_Item101_Active)
        //{
        //    // 확률 계산
        //    float tRandom = Random.Range(1f, 100f);
        //    if (_MissileChance >= tRandom)
        //    {
        //        // todo
        //        // 오브젝트풀링으로 변경
        //        Instantiate<GameObject>(_Missile, _MissilePos.transform.position, Quaternion.identity);
        //    }
        //}

        

        if (_Item100_Active)
        {
            int tRandom = Random.Range(0, 100);
            if(_BleedingChance > tRandom)
            {
                tEnemy.Bleeding(_BleedingDamage, _BleedingChance);

            }
        }

        if (_Item101_Active)
        {          
            // 확률 계산
            int tRandom = Random.Range(1, 100);
            if (_MissileChance >= tRandom)
            {
                // 아이템 생성
                GameObject tMissile = ObjectPool._Inst.GetObject("Missile", _MissilePos.position);
            }

        }
    }

    public virtual void Damaged(float tDamage)
    {
        _CurHp -= tDamage;

        _CurHp = Mathf.Clamp(_CurHp, 0, _TotalHp);

        UpdateHpBar(_CurHp);

        if (_CurHp <= 0)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {
        _GameSceneUI.EndingUI("실패");
    }

    protected virtual void UpdateHpBar(float tHp)
    {
        _PlayerUI.UpdateHpBarValue(tHp);
    }


    public virtual void UpdateMoney(int tMoney) 
    {
        _Money += tMoney;
        _GameSceneUI.UpdateMoneyUI(_Money);
    }

    protected void ChangeState(PlayerState tState)
    {
        _PlayerState = tState;
    }
}
