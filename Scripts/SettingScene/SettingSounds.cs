using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSounds : MonoBehaviour
{
    [SerializeField]
    AudioSource _MouseClickAudioSource;

    [SerializeField]
    AudioSource _MouseOverAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseClickSound()
    {
        _MouseClickAudioSource.Play();
    }

    public void MouseOverSound()
    {
        _MouseOverAudioSource.Play();
    }
}
