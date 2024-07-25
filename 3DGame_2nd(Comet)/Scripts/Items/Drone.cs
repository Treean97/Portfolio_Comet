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

        // ����� ���� �ΰ��� ����
        _BulletLineRenderer.positionCount = 2;
        // ���� �������� ��Ȱ��ȭ
        _BulletLineRenderer.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        // ��Ȱ��ȭ�� �ƴϸ�
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

        // �ݶ��̴� off
        gameObject.GetComponent<SphereCollider>().enabled = false;

        
    }

    void FindTarget()
    {
        // ������ ����� ����Ʈ �ʱ�ȭ
        _NearEnemyList.Clear();

        // ��Ÿ� �̳��� Enemy ���̾ ���� �ݶ��̴� ã��
        _NearEnemyList = Physics.OverlapSphere(_Player.transform.position, _AttackRange, _FindEnemyLayerMask).ToList<Collider>();

        // �±׷� ���� �ִ��� Ȯ�� �� ����Ʈ�� �ֱ� (���� ���̾��ũ�� �ذ�)
        //foreach (var t in _NearEnemyList)
        //{
        //    if (t.gameObject.CompareTag("Enemy"))
        //    {
        //        _EnemyList.Add(t.gameObject);
        //    }
        //}

        // ���� �ϳ� �̻� ������
        if (_NearEnemyList.Count != 0)
        {
            // ����Ʈ�� ù ��° ���, ������ �Ÿ� �ֱ�
            _Target = _NearEnemyList[0].gameObject;
            float tShortDis = Vector3.Distance(_Player.transform.position, _Target.transform.position);

            // ù ��° ���� ���ϸ� ���� ����� �� ã��
            foreach (var t in _NearEnemyList)
            {
                float tDis = Vector3.Distance(_Player.transform.position, t.transform.position);

                // ���� ��󺸴� �Ÿ��� ª�ٸ� ���� ���� ����
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

    void Rotate()
    {
        _Degree += Time.deltaTime;

        if (_Degree < 360)
        {
            transform.position =
                _Player.transform.position + (new Vector3(Mathf.Cos(_Degree), 0, Mathf.Sin(_Degree)) * _CircleRadius) + Vector3.up * _Height;


            // ��� �ٶ󺸱�
            if (_Target != null)
            {
                // ���� ���ϱ�
                Vector3 tDir = (_Target.transform.position - this.transform.position).normalized;

                // ȸ��
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

            // ���� ������
            StartCoroutine(FireCoroutine(_Target.transform.position));

            _CurAttackDelay = 0;
        }
        
    }

}
