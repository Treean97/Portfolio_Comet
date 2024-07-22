using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    Slider _HpBar;

    [SerializeField]
    TMP_Text _HpText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHpBarValue(float tHp)
    {
        _HpBar.value = tHp;
        UpdateHpText();
    }

    public void SetHpBarMaxValue(float tHp)
    {
        _HpBar.maxValue = tHp;
        UpdateHpText();
    }

    public void UpdateHpText()
    {
        StringBuilder tSB = new StringBuilder();

        tSB.Append(_HpBar.value);
        tSB.Append("/");
        tSB.Append(_HpBar.maxValue);

        _HpText.text = tSB.ToString();
    }
}
