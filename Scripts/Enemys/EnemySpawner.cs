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

    // 적 게임오브젝트 리스트
    [SerializeField]
    GameObject[] _EnemyGOArray;

    // 운석 게임오브젝트
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

    // x,z 좌표를 랜덤으로 정해서 위에서 아래로 ray를 쏜다
    // ray에 걸린 지면의 y축 좌표를 통해  x,y,z 값의 위치에 생성한다


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
            // 생성 위치 체크, 생성
            CheckSpawnPosition();

            
            // 생성 쿨타임
            yield return new WaitForSeconds(_SpawnDelay);
        }
    }

    void CheckSpawnPosition()
    {
        // 소환 할 position 랜덤 x, z 값 생성
        float tPositionX = Random.Range(-_SpawnLimitX, _SpawnLimitX);
        float tPositionZ = Random.Range(-_SpawnLimitZ, _SpawnLimitZ);

        // 도착할 포지션
        Vector3 tSpawnPosition = new Vector3(tPositionX, 0, tPositionZ);

        RaycastHit tHit;

        float tMaxRayDistance = 100;
        float RaycastYDistance = 50;

        // 소환 할 x,z 지점에 ray를 통해 지면의 y값 알아내기
        if(Physics.Raycast(tSpawnPosition + Vector3.up * RaycastYDistance, Vector3.down, out tHit, tMaxRayDistance))
        {
            tSpawnPosition.y = tHit.point.y;
        }

        // 낙하 시작 지점 역으로 구하기
        float tSpawnStartPositionX = Random.Range(-_SpawnStartRadius, _SpawnStartRadius);
        float tSpawnStartPositionZ = Random.Range(-_SpawnStartRadius, _SpawnStartRadius);

        Vector3 tSpawnStartPosition = tSpawnPosition + new Vector3(tSpawnStartPositionX, _SpawnStartHeight, tSpawnStartPositionZ);

        // 낙하 방향
        Vector3 tTargetVector = (tSpawnPosition - tSpawnStartPosition).normalized;

        EnemySpawn(tSpawnStartPosition, tTargetVector, tSpawnPosition);
    }

    void EnemySpawn(Vector3 tStartPos, Vector3 tTargetVec, Vector3 tSpawnPos)
    {
        // 추후 오브젝트 풀링으로 수정
        // 생성할 객체 선택
        int tEnemyIndex = Random.Range(0, _EnemyGOArray.Length);

        // 적 생성
        //GameObject tEnemy = Instantiate<GameObject>(_EnemyGOArray[tEnemyIndex], tStartPos, Quaternion.identity);

        // 운석 생성
        GameObject tRock = ObjectPool._Inst.GetObject("EnemySpawnRock", tStartPos);

        EnemySpawnRock tRockScript = tRock.GetComponent<EnemySpawnRock>();

        // 운석이 생성시킬 적 정보 넣기
        tRockScript.SetEnemyGameObject(_EnemyGOArray[tEnemyIndex]);

        // 낙하 벡터 전달
        tRockScript.SetTargetPos(tTargetVec);

        // 소환 할 위치 전달
        tRockScript.SetSpawnPos(tSpawnPos);

    }
}
