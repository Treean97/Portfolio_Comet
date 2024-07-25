using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneUI : MonoBehaviour
{
    [SerializeField]
    GameObject _CharacterSelectUIGO;

    [SerializeField]
    MainCameraRotate _Camera;

    [SerializeField]
    FadeController _FadeController;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        _FadeController.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickStartBtn()
    {
        if(!_CharacterSelectUIGO.activeSelf)
        {
            _CharacterSelectUIGO.SetActive(true);
        }        
    }

    public void OnClickSettingBtn()
    {
        _Camera.SaveCameraRot();
        _FadeController.FadeOutWithChangeScene("SettingScene");
    }


    public void OnClickQuitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
