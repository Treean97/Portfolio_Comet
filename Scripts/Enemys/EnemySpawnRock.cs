using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnemySpawnRock : MonoBehaviour
{
    EnemySpawner _EnemySpanwer;

    Vector3 _TargetPos;

    Vector3 _SpawnPos;

    [SerializeField]
    float _FallingSpeed;

    [SerializeField]
    float _MaxDissolveAmount;

    [SerializeField]
    float _CurDissolveAmount;

    [SerializeField]
    Renderer _Renderer;

    Material _Material;

    bool _IsOnGround = false;

    // �ٴ� üũ
    float _ColliderRadius;
    //float _CheckGroundRay = 0.5f;

    // ��ȯ�� ��ü
    GameObject _Enemy;

    [SerializeField]
    ParticleSystem _SmokeParticle;

    StringBuilder _StringBuilder;

    // Start is called before the first frame update
    void Start()
    {     
        _StringBuilder = new StringBuilder();
    }

    private void OnEnable()
    {
        if(_Material == null)
        {
            _Material = _Renderer.material;
        }
        if(_ColliderRadius == 0)
        {
            _ColliderRadius = GetComponent<SphereCollider>().radius;
        }
        if(_EnemySpanwer == null)
        {
            _EnemySpanwer = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        }

        GetComponent<SphereCollider>().enabled = true;
        _Material.SetFloat("_DissolveAmount", 0);
        _Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        _Renderer.receiveShadows = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_IsOnGround)
        {
            transform.Translate(_TargetPos.normalized * _FallingSpeed * Time.deltaTime);
            CheckGround();
        }

    }


    // ��ȯ�� �� ���� ����
    public void SetEnemyGameObject(GameObject tEnemy)
    {
        _Enemy = tEnemy;

    }

    // ���� ��ȯ�� ��ġ ����
    public void SetSpawnPos(Vector3 tSpawnPos)
    {
        _SpawnPos = tSpawnPos;
    }

    // ������ ���� ���� ����
    public void SetTargetPos(Vector3 tTargetPos)
    {
        _TargetPos = tTargetPos;
    }


    void CheckGround()
    {
        //if(Physics.Raycast(new Vector3(0, -_CheckPos, 0), Vector3.down, _CheckGroundRay))
        //{
        //    _IsOnGround = true;

        //    OnGround();
        //}

        // Debug.Log("Y : " + transform.position.y + ", SY : " + _SpawnPos.y);

        // ��ü �ݶ��̴��� �ٴ��� ��ȯ ������ ������
        if (transform.position.y - _ColliderRadius <= _SpawnPos.y)
        {
            _IsOnGround = true;
            OnGround();
        }

    }

    void OnGround()
    {
        _IsOnGround = true;

        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        /*
        1. �ݶ��̴� ����
        2. �� ������Ʈ ��ȯ
        3. Dissolve ���� (�׸��� ����)
        4. �� ������Ʈ ������ ����
        5. �� ������Ʈ ����
        */

        _SmokeParticle.Stop();

        GetComponent<SphereCollider>().enabled = false;

        _StringBuilder.Clear();
        _StringBuilder = _StringBuilder.Append("Enemy_").Append(_Enemy.GetComponent<Enemy>()._EnemyStatus.GetEnemyId);

        // �� ������Ʈ ��ȯ
        GameObject tEnemy = ObjectPool._Inst.GetObject(_StringBuilder.ToString());
        tEnemy.transform.position = _SpawnPos;

        // Dissolve & shadow off
        _Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _Renderer.receiveShadows = false;

        while (_CurDissolveAmount <= _MaxDissolveAmount)
        {
            _CurDissolveAmount += Time.deltaTime;

            _Material.SetFloat("_DissolveAmount", _CurDissolveAmount);

            yield return null;
        }

        // ������ �� �����̴� ���·� ����
        tEnemy.GetComponent<Enemy>().ChangeState(Enemy.STATE.Movement);

        // ������Ʈ �ݳ�
        ObjectPool._Inst.ReturnObject(this.gameObject);
    }
}
