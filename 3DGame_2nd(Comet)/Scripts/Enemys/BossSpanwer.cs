using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpanwer : MonoBehaviour
{
    // 보스 게임오브젝트 리스트
    [SerializeField]
    GameObject[] _BossGOArray;

    [SerializeField]
    GameSceneUI _GameSceneUI;

    [SerializeField]
    Transform _SpawnPos;

    bool _IsSpawn = false;

    [SerializeField]
    AudioSource _AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBoss()
    {
        if(_IsSpawn)
        {
            return;
        }

        _AudioSource.Play();

        _IsSpawn = true;

        // UI 활성화
        _GameSceneUI.ShowBossUI();

        // 보스 소환
        int tRandomIndex = Random.Range(0, _BossGOArray.Length);
        GameObject tBossGO = Instantiate<GameObject>(_BossGOArray[tRandomIndex], _SpawnPos.position, Quaternion.identity);
    }
}
