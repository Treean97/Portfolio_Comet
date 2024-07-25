using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr _Inst = null;

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

    [SerializeField]
    public AudioSource _AudioSource;

    [SerializeField]
    public AudioClip[] _AudioClip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(int tIdx)
    {
        _AudioSource.clip = _AudioClip[tIdx];
    }

}
