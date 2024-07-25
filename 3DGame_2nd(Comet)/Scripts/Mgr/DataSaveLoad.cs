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
        // 기본값
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
        // 시작 시 데이터 불러오기
        LoadData();
        LoadSoundVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData() // 데이터 저장
    {
        // 저장할 데이터
        _Data.XSens = GameManager._Inst._DataSaveLoad._Data.XSens;
        _Data.YSens = GameManager._Inst._DataSaveLoad._Data.YSens;

        _Data.BGMValue = GameManager._Inst._DataSaveLoad._Data.BGMValue;
        _Data.SoundEffectValue = GameManager._Inst._DataSaveLoad._Data.SoundEffectValue;

        // 저장하려는 클래스를 제이슨을 통해 문자열로 변환
        string saveData = JsonUtility.ToJson(_Data, false);

        // 저장하려는 위치 경로  
        string path = Application.persistentDataPath + "/PlayerData.json";

        File.WriteAllText(path, saveData);
    }

    public void LoadData()
    {
        // 불러오려는 위치 경로  
        string path = Application.persistentDataPath + "/PlayerData.json";

        if (File.Exists(path))
        {
            // 파일에 저장되어있던 문자열 가져오기
            string loadData = File.ReadAllText(path);

            // 가져온 문자열을 제이슨을 통해 클래스로 변환
            _Data = JsonUtility.FromJson<PlayerData>(loadData);            
        }
        
        // 최초 실행 시 새로운 객체 생성
        if(_Data == null)
        {
            _Data = new PlayerData();
        }

        // 불러올 데이터
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
