using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMgr : MonoBehaviour
{
    [SerializeField]
    GameObject[] _PlayerCharacters;

    public int _BuyCount;

    public int _DronePrice;

    [SerializeField]
    float _MouseXSens = 1f;
    public float GetMouseXSens { get { return _MouseXSens; } }

    [SerializeField]
    float _MouseYSens = 1f;
    public float GetMouseYSens { get { return _MouseYSens; } }

    [SerializeField]
    EnemySpawner _EnemySpawner;


    private void Awake()
    {
        if (GameManager._Inst != null)
        {
            // 게임매니저에서 설정값 가져오기
            this._MouseXSens = GameManager._Inst._DataSaveLoad._Data.XSens;
            this._MouseYSens = GameManager._Inst._DataSaveLoad._Data.YSens;
        }

        // 캐릭터 스폰
        CharacterSpawn();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        HideCursor();
        _EnemySpawner.StartSpawnCoroutine();

        ChangeBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeBGM()
    {
        if (SoundMgr._Inst._AudioSource.clip != SoundMgr._Inst._AudioClip[1])
        {
            SoundMgr._Inst.ChangeBGM(1);
            SoundMgr._Inst._AudioSource.Play();
        }
    }
    
    void CharacterSpawn()
    {
        int tPlayerNum = GameManager._Inst._PlayerStatus.GetPlayerId;
        Instantiate(_PlayerCharacters[tPlayerNum]);
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
