using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Missile : MonoBehaviour
{
    [SerializeField]
    ItemStatus _ItemStatus;

    [SerializeField]
    Rigidbody _Rigidbody;

    GameObject _Target;

    [SerializeField]
    [Range(0f, 1f)]
    float _RotateSpeed;

    [SerializeField]
    float _MoveForwardSpeed;

    [SerializeField]
    float _LaunchSpeed;

    [SerializeField]
    float _MaxFireSpeed;

    [SerializeField]
    float _CurFireSpeed;

    [SerializeField]
    float _Acceleration;

    [SerializeField]
    float _AttackPower;

    [SerializeField]
    float _ReTargetRange;

    [SerializeField]
    LayerMask _LayerMask;

    [SerializeField]
    List<Collider> _NearEnemyColliderList;

    [SerializeField]
    float _MaxDestroyDelay;

    GameSounds _GameSounds;

    enum MissileState
    {
        Launch,
        Fire,
        Destroy
    }

    [SerializeField]
    MissileState _MissileState;



    // Start is called before the first frame update
    void Start()
    {
        // 사운드 매니저
        _GameSounds = GameObject.FindObjectOfType<GameSounds>();

        _AttackPower = _ItemStatus.GetItemAdvValue;
        _NearEnemyColliderList = new List<Collider>();
        _MissileState = MissileState.Launch;
        Launch();

   
    }

    private void OnEnable()
    {
        Invoke("ReturnObject", _MaxDestroyDelay);
    }

    void ReturnObject()
    {
        ObjectPool._Inst.ReturnObject(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_MissileState)
        {
            case MissileState.Launch:
                Launch();
                break;

            case MissileState.Fire:
                SetTarget();
                break;

            case MissileState.Destroy:
                break;
        }
    }

    public void SetTarget()
    {
        // 가속
        _CurFireSpeed += Time.deltaTime * _Acceleration;

        // 속도 제한
        _CurFireSpeed = Mathf.Clamp(_CurFireSpeed, 0, _MaxFireSpeed);        

        // 이전에 사용한 리스트 초기화
        _NearEnemyColliderList.Clear();

        // 사거리 이내의 Enemy 레이어를 가진 콜라이더 찾기
        _NearEnemyColliderList = Physics.OverlapSphere(this.transform.position, _ReTargetRange, _LayerMask).ToList<Collider>();

        // 적이 하나 이상 있으면
        if (_NearEnemyColliderList.Count > 0)
        {
            // 리스트의 첫 번째 대상, 대상과의 거리 넣기
            _Target = _NearEnemyColliderList[0].gameObject;
            float tShortDis = Vector3.Distance(this.transform.position, _Target.transform.position);

            // 첫 번째 대상과 비교하며 가장 가까운 적 찾기
            foreach (var t in _NearEnemyColliderList)
            {
                float tDis = Vector3.Distance(this.transform.position, t.transform.position);

                // 이전 대상보다 거리가 짧다면 값을 새로 지정
                if (tDis <= tShortDis)
                {
                    tShortDis = tDis;
                    _Target = t.gameObject;
                }

            }

            Fire();
        }
        // 대상이 없으면
        else
        {
            // 직선이동
            MoveForward();
        }


    }

    void Launch()
    {
        _GameSounds.MissileLaunchSounds();

        _Rigidbody.velocity = Vector3.up * _LaunchSpeed;
        StartCoroutine(nameof(LaunchDelay));
    }

    IEnumerator LaunchDelay()
    {
        float tLaunchDelayTime = 0.5f;

        yield return new WaitForSeconds(tLaunchDelayTime);
        _MissileState = MissileState.Fire;
    }

    void Fire()
    {
        // 방향
        Vector3 tDir = (_Target.transform.position - transform.position).normalized;
        // 방향을 향하게
        // transform.up = Vector3.Lerp(transform.up, tDir, _RotateSpeed).normalized;

        // 테스트
        transform.up = tDir;

        MoveForward();
    }

    void MoveForward()
    {
        _Rigidbody.velocity = transform.up * _CurFireSpeed;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy tEnemy = collision.gameObject.GetComponent<Enemy>();
            tEnemy.Damaged(_AttackPower);
        }

        _GameSounds.MissileHitSounds();

        ObjectPool._Inst.ReturnObject(this.gameObject);
    }
}
