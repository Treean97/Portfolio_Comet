using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Audio;

public class DataSaveLoad : MonoBehaviour
{   
    public class PlayerData
    {
        // �⺻��
        public float XSens = 1;
        public float YSens = 1;

        public float BGMValue = 1;
        public float SoundEffectValue = 1;
    }

    public PlayerData _Data;

    [SerializeField]
    AudioMixer _AudioMixer;

    // Start is called before the first frame update
    void Start()
    {
        // ���� �� ������ �ҷ�����
        LoadData();
        LoadSoundVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData() // ������ ����
    {
        // ������ ������
        _Data.XSens = GameManager._Inst._DataSaveLoad._Data.XSens;
        _Data.YSens = GameManager._Inst._DataSaveLoad._Data.YSens;

        _Data.BGMValue = GameManager._Inst._DataSaveLoad._Data.BGMValue;
        _Data.SoundEffectValue = GameManager._Inst._DataSaveLoad._Data.SoundEffectValue;

        // �����Ϸ��� Ŭ������ ���̽��� ���� ���ڿ��� ��ȯ
        string saveData = JsonUtility.ToJson(_Data, false);

        // �����Ϸ��� ��ġ ���  
        string path = Application.persistentDataPath + "/PlayerData.json";

        File.WriteAllText(path, saveData);
    }

    public void LoadData()
    {
        // �ҷ������� ��ġ ���  
        string path = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(path))
        {
            // ���Ͽ� ����Ǿ��ִ� ���ڿ� ��������
            string loadData = File.ReadAllText(path);

            // ������ ���ڿ��� ���̽��� ���� Ŭ������ ��ȯ
            _Data = JsonUtility.FromJson<PlayerData>(loadData);            
        }
        
        // ���� ���� �� ���ο� ��ü ����
        if(_Data == null)
        {
            _Data = new PlayerData();
        }

        // �ҷ��� ������
        GameManager._Inst._DataSaveLoad._Data.XSens = _Data.XSens;
        GameManager._Inst._DataSaveLoad._Data.YSens = _Data.YSens;

        GameManager._Inst._DataSaveLoad._Data.BGMValue = _Data.BGMValue;
        GameManager._Inst._DataSaveLoad._Data.SoundEffectValue = _Data.SoundEffectValue;
    }

    public void LoadSoundVolume()
    {
        _AudioMixer.SetFloat("BGM", Mathf.Log10(_Data.BGMValue) * 20);
        _AudioMixer.SetFloat("SoundEffect", Mathf.Log10(_Data.SoundEffectValue) * 20);
    }
}
