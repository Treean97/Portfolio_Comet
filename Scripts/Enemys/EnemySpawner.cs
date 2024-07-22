using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float _SpawnLimitX;

    [SerializeField]
    float _SpawnLimitZ;

    [SerializeField]
    float _SpawnStartRadius;

    [SerializeField]
    float _SpawnStartHeight;

    Vector3 _SpawnPosition;

    // �� ���ӿ�����Ʈ ����Ʈ
    [SerializeField]
    GameObject[] _EnemyGOArray;

    // � ���ӿ�����Ʈ
    [SerializeField]
    GameObject _SpawnRock;

    Player _Player;

    [SerializeField]
    float _SpawnLimitDis;

    [SerializeField]
    float _SpawnDelay;

    [SerializeField]
    GameObject _EnemyGOList;

    [SerializeField]
    GameObject _EnemySpawnRockGOList;

    // x,z ��ǥ�� �������� ���ؼ� ������ �Ʒ��� ray�� ���
    // ray�� �ɸ� ������ y�� ��ǥ�� ����  x,y,z ���� ��ġ�� �����Ѵ�


    // Start is called before the first frame update
    void Start()
    {
        _Player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.O))
        //{
        //    StartSpawnCoroutine();
        //}

        //if (Input.GetKeyUp(KeyCode.P))
        //{
        //    StopSpawnCoroutine();
        //}
    }

    public void StartSpawnCoroutine()
    {
        StartCoroutine(nameof(SpawnEnemy));
    }

    public void StopSpawnCoroutine()
    {
        StopCoroutine(nameof(SpawnEnemy));
    }

    IEnumerator SpawnEnemy()
    {        

        while (true)
        {
            // ���� ��ġ üũ, ����
            CheckSpawnPosition();

            
            // ���� ��Ÿ��
            yield return new WaitForSeconds(_SpawnDelay);
        }
    }

    void CheckSpawnPosition()
    {
        // ��ȯ �� position ���� x, z �� ����
        float tPositionX = Random.Range(-_SpawnLimitX, _SpawnLimitX);
        float tPositionZ = Random.Range(-_SpawnLimitZ, _SpawnLimitZ);

        // ������ ������
        Vector3 tSpawnPosition = new Vector3(tPositionX, 0, tPositionZ);

        RaycastHit tHit;

        float tMaxRayDistance = 100;
        float RaycastYDistance = 50;

        // ��ȯ �� x,z ������ ray�� ���� ������ y�� �˾Ƴ���
        if(Physics.Raycast(tSpawnPosition + Vector3.up * RaycastYDistance, Vector3.down, out tHit, tMaxRayDistance))
        {
            tSpawnPosition.y = tHit.point.y;
        }

        // ���� ���� ���� ������ ���ϱ�
        float tSpawnStartPositionX = Random.Range(-_SpawnStartRadius, _SpawnStartRadius);
        float tSpawnStartPositionZ = Random.Range(-_SpawnStartRadius, _SpawnStartRadius);

        Vector3 tSpawnStartPosition = tSpawnPosition + new Vector3(tSpawnStartPositionX, _SpawnStartHeight, tSpawnStartPositionZ);

        // ���� ����
        Vector3 tTargetVector = (tSpawnPosition - tSpawnStartPosition).normalized;

        EnemySpawn(tSpawnStartPosition, tTargetVector, tSpawnPosition);
    }

    void EnemySpawn(Vector3 tStartPos, Vector3 tTargetVec, Vector3 tSpawnPos)
    {
        // ���� ������Ʈ Ǯ������ ����
        // ������ ��ü ����
        int tEnemyIndex = Random.Range(0, _EnemyGOArray.Length);

        // �� ����
        //GameObject tEnemy = Instantiate<GameObject>(_EnemyGOArray[tEnemyIndex], tStartPos, Quaternion.identity);

        // � ����
        GameObject tRock = ObjectPool._Inst.GetObject("EnemySpawnRock", tStartPos);

        EnemySpawnRock tRockScript = tRock.GetComponent<EnemySpawnRock>();

        // ��� ������ų �� ���� �ֱ�
        tRockScript.SetEnemyGameObject(_EnemyGOArray[tEnemyIndex]);

        // ���� ���� ����
        tRockScript.SetTargetPos(tTargetVec);

        // ��ȯ �� ��ġ ����
        tRockScript.SetSpawnPos(tSpawnPos);

    }
}
