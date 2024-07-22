using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _ResultText;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateResultText(string tEndingType)
    {
        _ResultText.text = tEndingType;
    }

    public void OnClickGoMainBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
}
