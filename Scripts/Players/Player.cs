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
    // �÷��̾� ����
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

    // �÷��̾� �������ͽ�
    [SerializeField]
    public PlayerStatus _PlayerStatus;

    // �÷��̾� �Ѿ� �������ͽ�
    [SerializeField]
    public BulletStatus _PlayerBullet_Normal_Status;

    // �÷��̾� ġ��Ÿ �Ѿ� �������ͽ�
    [SerializeField]
    public BulletStatus _PlayerBullet_Cri_Status;

    [SerializeField]
    Camera _MainCamera;

    [SerializeField]
    GameSceneMgr _GameSceneMgr;

    [SerializeField]
    protected CharacterController _CController;

    // Raycast ����
    [SerializeField]
    Transform _RaycastPos;

    // �߻� ����
    [SerializeField]
    Transform _FirePos;

    // �̻��� �߻� ����
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

    // ������
    [SerializeField]
    protected float _Gravity;

    protected Vector3 _MoveDir;

    protected float _y;

    // �÷��̾� �������ͽ�
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

    // �κ��丮
    [SerializeField]
    Dictionary<int, int> _PlayerInventory = new Dictionary<int, int>();

    public Dictionary<int, int> GetPlayerInventory { get { return _PlayerInventory; } }

    // ������ ���� üũ
    [SerializeField]
    bool _Item100_Active = false;

    [SerializeField]
    bool _Item101_Active = false;

    // ������ ȿ��
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

    // �� ź����
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


    // �÷��̾� ���� ����
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

        // ����� ���� �ΰ��� ����
        _BulletLineRenderer.positionCount = 2;
        // ���� �������� ��Ȱ��ȭ
        _BulletLineRenderer.enabled = false;

        // ���� �Ŵ���
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
        // �� �׽�Ʈ
        if(Input.GetKeyDown(KeyCode.P))
        {
            UpdateMoney(1000);
        }

        // �κ��丮 Ȯ�� �׽�Ʈ
        if(Input.GetKeyDown(KeyCode.I))
        {
            foreach (KeyValuePair<int, int> kvp in _PlayerInventory)
            {
                Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
            }
        }

        // ������ �׽�Ʈ
        if(Input.GetKeyDown(KeyCode.M))
        {
            ObjectPool._Inst.GetObject("Missile");
        }


        GroundCheckRay();

        if (_IsGround)
        {
            _CurJumpCount = 0;

            // ������ ���� �� ���� �ߴٸ�
            if(_PlayerState == PlayerState.Jump)
            {
                ChangeState(PlayerState.Idle);

                // ����
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
        // �Է°�
        float tHorizonal = Input.GetAxisRaw("Horizontal");
        float tVertical = Input.GetAxisRaw("Vertical");

        // ���� ���� �� ����ȭ
        _MoveDir = new Vector3(tHorizonal, 0, tVertical);
        _MoveDir = _MoveDir.normalized;

        // ���� ���̰ų� �޸��� ���� �ƴ� ��
        if(_PlayerState != PlayerState.Jump)
        {
            // ���� �Է��� ������ (�����̴� ��)
            if (_MoveDir.magnitude != 0)
            {
                // �޸��� ���� �ƴ϶��
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


        // ��ȯ
        Vector3 _MoveWorldDir = this.transform.TransformDirection(_MoveDir);

        // y��(���� or �߷�) ����
        _MoveWorldDir.y = _y;

        // ������
        _CController.Move(_MoveWorldDir * _ApplySpeed * Time.deltaTime);

        // ī�޶�� ������ ����
        Vector3 lookForward = new Vector3(_MainCamera.transform.forward.x, 0f, _MainCamera.transform.forward.z).normalized;
        this.transform.forward = lookForward;
    }

    /* %���·� �Է�(ex 50% = 50, -50% = -50) */
    protected virtual void CalApplySpeedMul(float tVar = 1)
    {        
        _ApplySpeed += _TotalSpeed * (tVar * 0.01f);
    }

    /* +���·� �Է�(ex +50 = 50, -50 = -50)*/
    protected virtual void CalApplySpeedPlus(float tVar = 0)
    {
        _ApplySpeed += _TotalSpeed + tVar;
    }

    protected virtual void PlayerJump()
    {
        ChangeState(PlayerState.Jump);
        // ����
        _GameSounds.JumpStartSounds();

        // y�� ������ �Է�
        _y = _PlayerStatus.GetPlayerJumpPower + _TotalJumpPower;

        // ���� ī��Ʈ ����
        _CurJumpCount++;
    }

    protected virtual void CheckItemRaycast()
    {
        if (Physics.Raycast(_RaycastPos.position, transform.forward, out RaycastHit tHit, _ItemCheckRaycastRange, _ItemLayerMask))
        {
            // ����� �������̸�
            if (tHit.collider.CompareTag("Item"))
            {
                // ������ UI On, �ٸ� UI�� �����ִٸ� Off
                if (_GameSceneUI._ItemUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._ItemUIGO);
                }

                // UI��������
                Items tItem = tHit.collider.GetComponent<Items>();
                _GameSceneUI.UpdateItemUI(tItem._ItemStatus);

                // ������ ȹ��
                if (Input.GetKeyDown(KeyCode.E))
                {
                    AddItem(tItem._ItemStatus);

                    // ������ ���� UI ����
                    _GameSceneUI.HideItemUI();

                    tItem.OnDestory();
                }
            }

            // ����� ������ �������̸�
            else if(tHit.collider.CompareTag("SelectItem"))
            {
                if(_GameSceneUI._SelectItemText.gameObject.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._SelectItemText.gameObject);
                }

                // ������ ���� �ؽ�Ʈ UI On
                if (_GameSceneUI._ItemBuyUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._SelectItemText.gameObject);
                }

                // �Ͻ����� ���°� �ƴϸ� ȹ�� ����
                if (Input.GetKeyDown(KeyCode.E) && _GameSceneUI._UIState != GameSceneUI.UIState.Running)
                {
                    _GameSceneUI.SelectItemUIOnOff();                                       

                    // ������ ����
                    Destroy(tHit.collider.gameObject);
                }
            }

            // ����� ������ �������
            else if (tHit.collider.CompareTag("ItemSpawner"))
            {
                // �ʿ� ��ȭ ���
                int tNeedMoney = _GameSceneMgr._BuyCount * 10;

                // ������ ���� UI ����
                _GameSceneUI.UpdateBuyItemUI(tNeedMoney);

                // ������ ���� UI On
                if (_GameSceneUI._ItemBuyUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._ItemBuyUIGO);
                }

                // �Ͻ����� ���°� �ƴϸ� ���� ����
                if (Input.GetKeyDown(KeyCode.E) && _GameSceneUI._UIState != GameSceneUI.UIState.Running)
                {
                    // ��ȭ�� ����ϸ�
                    if (_Money >= tNeedMoney)
                    {
                        // ������ ���濡 ������ ����
                        _ItemMgr.SpawnItem(tHit.transform.parent.gameObject);

                        // BuyCount����
                        _GameSceneMgr._BuyCount++;

                        // ��ȭ ����
                        UpdateMoney(-tNeedMoney);
                    }
                    // ��ȭ����UI
                    else
                    {
                        _GameSceneUI.WarningTextUI("Not Enough Money");
                    }

                }
            }

            // ����� ����̸�
            else if (tHit.collider.CompareTag("Drone"))
            {
                // �ʿ� ��ȭ ���
                int tNeedMoney = _GameSceneMgr._DronePrice;

                // ������ ���� UI ����
                _GameSceneUI.UpdateBuyItemUI(tNeedMoney);

                // ������ ���� UI On
                if (_GameSceneUI._ItemBuyUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._ItemBuyUIGO);
                }

                // �Ͻ����� ���°� �ƴϸ� ���� ����
                if (Input.GetKeyDown(KeyCode.E) && _GameSceneUI._UIState != GameSceneUI.UIState.Running)
                {
                    // ��ȭ�� ����ϸ�
                    if (_Money >= tNeedMoney)
                    {
                        // ��� �۵�
                        Drone tDrone = tHit.collider.GetComponentInParent<Drone>();
                        tDrone.ActiveDrone();

                        // ��ȭ ����
                        UpdateMoney(-tNeedMoney);
                    }
                    // ��ȭ����UI
                    else
                    {
                        _GameSceneUI.WarningTextUI("Not Enough Money");
                    }

                }
            }

            // ����� ���� �������
            else if(tHit.collider.CompareTag("BossSpawner"))
            {
                // UI On
                if (_GameSceneUI._BossSpawnUIGO.activeSelf == false)
                {
                    _GameSceneUI.HideCantDupUIWithout(_GameSceneUI._BossSpawnUIGO);
                }

                // �Ͻ����� ���°� �ƴϸ� ���� ���� ����
                if (Input.GetKeyDown(KeyCode.E) && _GameSceneUI._UIState != GameSceneUI.UIState.Running)
                {
                    // ���� ��ȯ
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
        // ����
        _GameSounds.GunSounds();

        _CurAttackDelay = 0;

        //Vector3 tRayDir = _MainCamera.ScreenPointToRay(Input.mousePosition).direction;

        //Ray tRay = new Ray(_FirePos.position, tRayDir);

        //RaycastHit tHit;

        //if (Physics.Raycast(tRay, out tHit, Mathf.Infinity, _IgnoreLayerMask))
        //{
        //    // ����
        //    var tDir = tHit.point - _FirePos.transform.position;

        //    var tBullets = ObjectPoolMgr.GetObject(); // Instantiate(bulletPrefab, transform.position + direction.normalized, Quaternion.identity).GetComponent<Bullet>();
        //    tBullets.transform.position = _FirePos.position + tDir.normalized;

        //    tBullets.DamageCal(_DamageMultiply);

        //    // ȿ�� üũ
        //    CheckPassiveItemIsActive(tBullets);

        //    tBullets.Shoot(tDir.normalized);

        //}

        // LineRenderer����
        // ź���� ����

        // Vector3 tTarget = _MainCamera.ScreenPointToRay(Input.mousePosition).direction;
        var tRay = _MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        RaycastHit tHit;

        Vector3 tAimPoint = Vector3.zero;

        if (Physics.Raycast(tRay, out tHit, Mathf.Infinity, _IgnoreLayerMask))
        {
            tAimPoint = tHit.point;
        }

        var tFixTarget = tAimPoint - _FirePos.position;

        // ���� ����
        float tRandomSpread = Random.Range(- _CurSpread / 2, _CurSpread / 2);
        tFixTarget = Quaternion.AngleAxis(tRandomSpread, Vector3.up) * tFixTarget;

        // �¿� ����
        tRandomSpread = Random.Range(-_CurSpread / 2, _CurSpread / 2);
        tFixTarget = Quaternion.AngleAxis(tRandomSpread, Vector3.right) * tFixTarget;


        if (Physics.Raycast(_FirePos.transform.position, tFixTarget, out tHit, Mathf.Infinity, _IgnoreLayerMask))
        {
            float tDamage = 0;

            // ġ��Ÿ �Ǵ�
            // ġ��Ÿ O
            if (IsCritical())
            {
                tDamage = _PlayerBullet_Cri_Status.GetBulletPower;
            }
            else
            {
                tDamage = _PlayerBullet_Normal_Status.GetBulletPower;
            }

            // ���� ������ ���
            tDamage *= (_DamageMultiply / 100);

            // ���̸�
            if(tHit.transform.CompareTag("Enemy"))
            {
                // ������ �ֱ�
                Enemy tEnemy = tHit.transform.GetComponent<Enemy>();

                tEnemy.Damaged(tDamage);

                //_DamageFontMgr.DamageUICheack(tDamage, tHit.point);
                // _DamageFontMgr.DamageUIObjectPool(tDamage, tHit.point);

                // �нú� ���
                CheckPassiveItemIsActive(tEnemy);
            }

            // ���� ������
            StartCoroutine(FireCoroutine(tHit.point));

        }
    }

    IEnumerator FireCoroutine(Vector3 tHitPos)
    {
        // ���� �������� �ѱ��� ��ġ
        _BulletLineRenderer.SetPosition(0, _FirePos.position);
        // ���� ������ �Է����� ���� �浹 ��ġ
        _BulletLineRenderer.SetPosition(1, tHitPos);
        // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���
        _BulletLineRenderer.enabled = true;

        // 0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� �Ѿ� ������ �����
        _BulletLineRenderer.enabled = false;
    }

    // ġ��Ÿ ���
    bool IsCritical()
    {
        // ġ��Ÿ O
        if (Random.Range(0, 100) <= _PlayerCriChance)
        {
            return true;
        }
        // ġ��Ÿ X
        else
        {
            return false;
        }        
    }

    // ������ ȹ��
    public virtual void AddItem(ItemStatus tItemStatus)
    {
        // �κ��丮�� ������ Ȯ��
        // ������
        if (_PlayerInventory.ContainsKey(tItemStatus.GetItemId))
        {
            // ������ 1�� �߰�
            _PlayerInventory[tItemStatus.GetItemId] += 1;
        }
        // ������
        else
        {
            // ������id, ���� 1�� �߰�
            _PlayerInventory.Add(tItemStatus.GetItemId, 1);
        }

        UpdateStatus(tItemStatus);
    }

    // �������� ���� Status ����
    protected virtual void UpdateStatus(ItemStatus tItemStatus)
    {
        // ������ Ÿ�� Ȯ��
        switch ((int)tItemStatus.GetItemType)
        {
            // Status ���� ������
            case 0:
                CheckStatusItem(tItemStatus);
                break;

            // Passive ������
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
                // ���� �ӵ��� ����
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

    // Passive Ÿ�� ������ üũ
    protected virtual void CheckPassiveItem(ItemStatus tItemStatus)
    {
        switch (tItemStatus.GetItemId)
        {
            case 100:
                // ���� ȹ�� �� ���� ȿ�� Ȱ��ȭ
                if (_PlayerInventory.ContainsKey(tItemStatus.GetItemId))
                {
                    _Item100_Active = true;
                }

                // ���� Ȯ�� ����
                _BleedingChance += tItemStatus._GetPassiveChance;

                // Ȯ�� ���� ����
                _BleedingChance = Mathf.Clamp(_BleedingChance, 0, 100);

                // ���� ������ ����
                _BleedingDamage += tItemStatus.GetItemAdvValue;

                break;

            case 101:
                // ���� ȹ�� �� �̻��� �߻� ȿ�� Ȱ��ȭ
                if (_PlayerInventory.ContainsKey(tItemStatus.GetItemId))
                {
                    _Item101_Active = true;
                }

                // �߻� Ȯ�� �߰�
                _MissileChance += tItemStatus._GetPassiveChance;

                // Ȯ�� ���� ����
                _MissileChance = Mathf.Clamp(_MissileChance, 0, 100);
                break;
        }

    }

    // ������ Ȱ��ȭ üũ
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
        //    // Ȯ�� ���
        //    float tRandom = Random.Range(1f, 100f);
        //    if (_MissileChance >= tRandom)
        //    {
        //        // todo
        //        // ������ƮǮ������ ����
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
            // Ȯ�� ���
            int tRandom = Random.Range(1, 100);
            if (_MissileChance >= tRandom)
            {
                // ������ ����
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
        _GameSceneUI.EndingUI("����");
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
