using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField]
    Transform _FirePos;

    [SerializeField]
    LineRenderer _BulletLineRenderer;

    enum DroneState
    {
        Inactive,
        Active
    }

    [SerializeField]
    float _TotalAttackDelay;

    [SerializeField]
    float _CurAttackDelay;

    [SerializeField]
    float _AttackRange;

    [SerializeField]
    BulletStatus _DroneBullet_Status;

    [SerializeField]
    GameObject _Target;

    [SerializeField]
    GameObject _Player;

    [SerializeField]
    float _CircleRadius;

    [SerializeField]
    float _Degree;

    [SerializeField]
    float _RotateSpeed;

    [SerializeField]
    float _Height;

    [SerializeField]
    [Range(0f, 1f)]
    float _Speed;

    [SerializeField]
    DroneState _DroneState = new DroneState();

    [SerializeField]
    LayerMask _FindEnemyLayerMask;

    List<Collider> _NearEnemyList;

    [SerializeField]
    AudioSource _AudioSource;

    [SerializeField]
    AudioClip[] _AudioClip;

    void Start()
    {
        _DroneState = DroneState.Inactive;
        _NearEnemyList = new List<Collider>();
        _Player = FindObjectOfType<Player>().gameObject;

        _BulletLineRenderer = GetComponent<LineRenderer>();

        // 사용할 점을 두개로 변경
        _BulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        _BulletLineRenderer.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        // 비활성화가 아니면
        if(_DroneState != DroneState.Inactive)
        {
            FindTarget();
            Rotate();
                         
            if(_CurAttackDelay <= _TotalAttackDelay)
            {
                _CurAttackDelay += Time.deltaTime;
            }
        }

    }

    public void ActiveDrone()
    {
        _DroneState = DroneState.Active;

        _AudioSource.clip = _AudioClip[0];
        _AudioSource.Play();

        // 콜라이더 off
        gameObject.GetComponent<SphereCollider>().enabled = false;

        
    }

    void FindTarget()
    {
        // 이전에 사용한 리스트 초기화
        _NearEnemyList.Clear();

        // 사거리 이내의 Enemy 레이어를 가진 콜라이더 찾기
        _NearEnemyList = Physics.OverlapSphere(_Player.transform.position, _AttackRange, _FindEnemyLayerMask).ToList<Collider>();

        // 태그로 적이 있는지 확인 후 리스트에 넣기 (위의 레이어마스크로 해결)
        //foreach (var t in _NearEnemyList)
        //{
        //    if (t.gameObject.CompareTag("Enemy"))
        //    {
        //        _EnemyList.Add(t.gameObject);
        //    }
        //}

        // 적이 하나 이상 있으면
        if (_NearEnemyList.Count != 0)
        {
            // 리스트의 첫 번째 대상, 대상과의 거리 넣기
            _Target = _NearEnemyList[0].gameObject;
            float tShortDis = Vector3.Distance(_Player.transform.position, _Target.transform.position);

            // 첫 번째 대상과 비교하며 가장 가까운 적 찾기
            foreach (var t in _NearEnemyList)
            {
                float tDis = Vector3.Distance(_Player.transform.position, t.transform.position);

                // 이전 대상보다 거리가 짧다면 값을 새로 지정
                if (tDis <= tShortDis)
                {
                    tShortDis = tDis;
                    _Target = t.gameObject;
                }

            }

            Attack();
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

    void Rotate()
    {
        _Degree += Time.deltaTime;

        if (_Degree < 360)
        {
            transform.position =
                _Player.transform.position + (new Vector3(Mathf.Cos(_Degree), 0, Mathf.Sin(_Degree)) * _CircleRadius) + Vector3.up * _Height;


            // 대상 바라보기
            if (_Target != null)
            {
                // 방향 구하기
                Vector3 tDir = (_Target.transform.position - this.transform.position).normalized;

                // 회전
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(tDir), 0.2f);

                // transform.LookAt(_Target.transform.position);
            }
            else
            {
                transform.Rotate(Vector3.zero);
            }            

        }
        else
        {
            _Degree = 0;
        }

    }

    void Attack()
    {
        if(_CurAttackDelay >= _TotalAttackDelay)
        {
            _AudioSource.clip = _AudioClip[1];
            _AudioSource.Play();

            _Target.GetComponent<Enemy>().Damaged(_DroneBullet_Status.GetBulletPower);

            // 라인 랜더러
            StartCoroutine(FireCoroutine(_Target.transform.position));

            _CurAttackDelay = 0;
        }
        
    }

}
