using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _MouseXSensValueText;
    [SerializeField]
    TMP_Text _MouseYSensValueText; 
    [SerializeField]
    TMP_Text _BGMValueText;
    [SerializeField]
    TMP_Text _SoundEffectText;

    [SerializeField]
    Slider _MouseXSensSlider;
    [SerializeField]
    Slider _MouseYSensSlider;
    [SerializeField]
    Slider _BGMValueSlider;
    [SerializeField]
    Slider _SoundEffectValueSlider;


    [SerializeField]
    GameObject _SettingsUI;

    [SerializeField]
    FadeController _FadeController;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Data�� ����� �� -> GameManager�� �ε� �� ��������
        _MouseXSensSlider.value = GameManager._Inst._DataSaveLoad._Data.XSens;
        _MouseYSensSlider.value = GameManager._Inst._DataSaveLoad._Data.YSens;
        _BGMValueSlider.value = GameManager._Inst._DataSaveLoad._Data.XSens;
        _SoundEffectValueSlider.value = GameManager._Inst._DataSaveLoad._Data.YSens;
    }

    // Update is called once per frame
    void Update()
    {
        _MouseXSensValueText.text = _MouseXSensSlider.value.ToString("F1");
        _MouseYSensValueText.text = _MouseYSensSlider.value.ToString("F1");
        _BGMValueText.text = _BGMValueSlider.value.ToString("F1");
        _SoundEffectText.text = _SoundEffectValueSlider.value.ToString("F1");
    }

    // �����Ϳ� �� �Է�
    public void OnClickApplyBtn()
    {
        GameManager._Inst._DataSaveLoad._Data.XSens = _MouseXSensSlider.value;
        GameManager._Inst._DataSaveLoad._Data.YSens = _MouseYSensSlider.value;
        GameManager._Inst._DataSaveLoad._Data.BGMValue = _BGMValueSlider.value;
        GameManager._Inst._DataSaveLoad._Data.SoundEffectValue = _SoundEffectValueSlider.value;

        // ������ ����
        GameManager._Inst._DataSaveLoad.SaveData();
    }

    public void OnClickSettingsUIOffBtn()
    {
        _SettingsUI.SetActive(false);
    }
}
