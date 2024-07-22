using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneMgr : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ChangeBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeBGM()
    {
        if (SoundMgr._Inst._AudioSource.clip != SoundMgr._Inst._AudioClip[0])
        {
            SoundMgr._Inst.ChangeBGM(0);
            SoundMgr._Inst._AudioSource.Play();
        }
    }
}
