using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageFontMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //public void DamageUICheack(float tDamage, Vector3 tTargetPos)
    //{
    //    // 사용 가능한 데미지 텍스트 검색
    //    for (int i = 0; i < _DamageTexts.Length; i++)
    //    {
    //        if (_DamageTexts[i].gameObject.activeSelf == false)
    //        {
    //            TMP_Text tDamageText = _DamageTexts[i];
    //            DamageUIOn(tDamageText, tDamage, tTargetPos);

    //            break;
    //        }
    //    }

    //}

    //void DamageUIOn(TMP_Text tDamageText, float tDamage, Vector3 tTargetPos)
    //{
    //    tDamageText.text = tDamage.ToString("F0");
    //    tDamageText.GetComponent<DamageFontAnimation>().SetTargetPos(tTargetPos);
    //    tDamageText.gameObject.SetActive(true);
    //}

    public void DamageUIObjectPool(float tDamage, Vector3 tTargetPos)
    {
        GameObject tDamageText = ObjectPool._Inst.GetObject("DamageText");
        tDamageText.transform.position = tTargetPos;
        tDamageText.GetComponent<TMP_Text>().text = tDamage.ToString("F0");
    }
}
