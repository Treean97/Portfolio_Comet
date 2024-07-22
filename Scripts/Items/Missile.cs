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
        // ���� �Ŵ���
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
        // ����
        _CurFireSpeed += Time.deltaTime * _Acceleration;

        // �ӵ� ����
        _CurFireSpeed = Mathf.Clamp(_CurFireSpeed, 0, _MaxFireSpeed);        

        // ������ ����� ����Ʈ �ʱ�ȭ
        _NearEnemyColliderList.Clear();

        // ��Ÿ� �̳��� Enemy ���̾ ���� �ݶ��̴� ã��
        _NearEnemyColliderList = Physics.OverlapSphere(this.transform.position, _ReTargetRange, _LayerMask).ToList<Collider>();

        // ���� �ϳ� �̻� ������
        if (_NearEnemyColliderList.Count > 0)
        {
            // ����Ʈ�� ù ��° ���, ������ �Ÿ� �ֱ�
            _Target = _NearEnemyColliderList[0].gameObject;
            float tShortDis = Vector3.Distance(this.transform.position, _Target.transform.position);

            // ù ��° ���� ���ϸ� ���� ����� �� ã��
            foreach (var t in _NearEnemyColliderList)
            {
                float tDis = Vector3.Distance(this.transform.position, t.transform.position);

                // ���� ��󺸴� �Ÿ��� ª�ٸ� ���� ���� ����
                if (tDis <= tShortDis)
                {
                    tShortDis = tDis;
                    _Target = t.gameObject;
                }

            }

            Fire();
        }
        // ����� ������
        else
        {
            // �����̵�
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
        // ����
        Vector3 tDir = (_Target.transform.position - transform.position).normalized;
        // ������ ���ϰ�
        // transform.up = Vector3.Lerp(transform.up, tDir, _RotateSpeed).normalized;

        // �׽�Ʈ
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
