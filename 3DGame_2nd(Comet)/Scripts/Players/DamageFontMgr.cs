using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageFontMgr : MonoBehaviour
{
    [SerializeField]
    Color _NormalTextColor;

    [SerializeField]
    Color _DrainTextColor;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DamageUIObjectPool(float tDamage, Vector3 tTargetPos)
    {
        GameObject tDamageText = ObjectPool._Inst.GetObject("DamageText");
        tDamageText.transform.position = tTargetPos;
        TMP_Text tText = tDamageText.GetComponent<TMP_Text>();
        tText.color = _NormalTextColor;
        tText.text = tDamage.ToString("F0");
    }

    public void DrainUIObjectPool(float tDamage, Vector3 tTargetPos)
    {
        GameObject tDamageText = ObjectPool._Inst.GetObject("DamageText");
        tDamageText.transform.position = tTargetPos;
        TMP_Text tText = tDamageText.GetComponent<TMP_Text>();
        tText.color = _DrainTextColor;
        tText.text = tDamage.ToString("F0");
    }

}
