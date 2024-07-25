using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpanwer : MonoBehaviour
{
    // ���� ���ӿ�����Ʈ ����Ʈ
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

        // UI Ȱ��ȭ
        _GameSceneUI.ShowBossUI();

        // ���� ��ȯ
        int tRandomIndex = Random.Range(0, _BossGOArray.Length);
        GameObject tBossGO = Instantiate<GameObject>(_BossGOArray[tRandomIndex], _SpawnPos.position, Quaternion.identity);
    }
}
