using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    [SerializeField]
    AudioSource _GunSounds;

    [SerializeField]
    AudioSource _FootStepSounds;

    [SerializeField]
    AudioSource _JumpSounds;

    [SerializeField]
    AudioClip[] _FootStepWalkSoundClip;

    [SerializeField]
    AudioClip[] _FootStepRunSoundClip;

    [SerializeField]
    AudioClip[] _JumpSoundClip;

    [SerializeField]
    AudioSource _MouseOverSounds;

    [SerializeField]
    AudioSource _MissileLaunchSounds;

    [SerializeField]
    AudioSource _MissileHitSounds;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GunSounds()
    {
        
        _GunSounds.Play();
    }

    public void MissileLaunchSounds()
    {
        _MissileLaunchSounds.Play();
    }

    public void MissileHitSounds()
    {
        _MissileHitSounds.Play();
    }

    public void WalkSounds()
    {
        int tRandom = Random.Range(0, _FootStepWalkSoundClip.Length);
        _FootStepSounds.clip = _FootStepWalkSoundClip[tRandom];
        
        _FootStepSounds.Play();
    }

    public void RunSounds()
    {
        int tRandom = Random.Range(0, _FootStepRunSoundClip.Length);
        _FootStepSounds.clip = _FootStepRunSoundClip[tRandom];
        
        _FootStepSounds.Play();
    }

    public void JumpStartSounds()
    {
        _JumpSounds.clip = _JumpSoundClip[0];
        _JumpSounds.Play();
    }

    public void JumpEndSounds()
    {
        _JumpSounds.clip = _JumpSoundClip[1];
        _JumpSounds.Play();
    }

    public void MouseClickSound()
    {
        _MouseOverSounds.Play();
    }

    public void MouseOverSound()
    {
        _MouseOverSounds.Play();

    }

    
}
