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

    // ĳ���� ���� �� ĳ���� ������ �Ѱ��� ����
    public PlayerStatus _PlayerStatus;
    
    private void Start()
    {
        
    }
    
}
