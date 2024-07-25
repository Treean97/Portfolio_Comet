using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerMgr : MonoBehaviour
{
    [SerializeField]
    AudioMixer _AudioMixer;

    [SerializeField]
    Slider _BGMSlider;

    [SerializeField]
    Slider _SoundEffectSlider;

    private void Start()
    {
        _BGMSlider.onValueChanged.AddListener(BGMControl);
        _SoundEffectSlider.onValueChanged.AddListener(SoundEffectControl);        
    }

    public void BGMControl(float tValue)
    {
        _AudioMixer.SetFloat("BGM", Mathf.Log10(tValue) * 20);

    }

    public void SoundEffectControl(float tValue)
    {
        _AudioMixer.SetFloat("SoundEffect", Mathf.Log10(tValue) * 20);

    }
}
