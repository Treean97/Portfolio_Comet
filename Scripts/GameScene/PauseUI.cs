using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    GameObject SettingsUI;

    [SerializeField]
    GameObject SlotInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        SlotInfo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickGoMainBtn()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickSettingBtn()
    {
        SettingsUI.SetActive(true);
    }
}
