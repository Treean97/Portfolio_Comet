using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Inst = null;

    [SerializeField]
    public DataSaveLoad _DataSaveLoad;

    public Quaternion _CameraRot;

    private void Awake()
    {
        if (_Inst == null)
        {
            _Inst = this; 
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // 캐릭터 선택 시 캐릭터 정보를 넘겨줄 변수
    public PlayerStatus _PlayerStatus;
    
    private void Start()
    {
        
    }
    
}
