using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMgr : MonoBehaviour
{
    [SerializeField]
    Sprite[] _Skill_0_Sprites;

    [SerializeField]
    GameObject[] _Skill_Slots;

    [SerializeField]
    Image[] _Skill_CoolTimeUIs;

    [SerializeField]
    Image[] _Skill_DurationTimeUIs;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _Skill_Slots.Length; i++)
        {
            GameObject tGO = _Skill_Slots[i].transform.Find("Skill_Icon").gameObject;
            tGO.GetComponent<Image>().sprite = _Skill_0_Sprites[GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()._PlayerStatus.GetPlayerId];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void Skill_0_Duration(float tSkill_0_MaxCoolTime, float tSkill_0_MaxDurationTime)
    //{
    //    StartCoroutine(Skill_0_CoolTimeCoroutine(tSkill_0_MaxCoolTime));
    //    StartCoroutine(Skill_0_DurationCoroutine(tSkill_0_MaxDurationTime));
    //}

    //public void Skill_0_Infinite_On(float tSkill_0_MaxCoolTime)
    //{
    //    StartCoroutine(Skill_0_CoolTimeCoroutine(tSkill_0_MaxCoolTime));
    //    _Skill_0_DurationTimeUI.fillAmount = 1;
    //}

    //public void Skill_0_Infinite_Off()
    //{
    //    _Skill_0_DurationTimeUI.fillAmount = 0;
    //}

    //IEnumerator Skill_0_DurationCoroutine(float tMaxDurationTime)
    //{

    //    float tSkillCurDurationTime = 0;

    //    while (tMaxDurationTime >= tSkillCurDurationTime)
    //    {
    //        _Skill_0_DurationTimeUI.fillAmount = 1 - (tSkillCurDurationTime / tMaxDurationTime);
    //        tSkillCurDurationTime += Time.deltaTime;
    //        yield return null;
    //    }
        
    //}

    //IEnumerator Skill_0_CoolTimeCoroutine(float tMaxCoolTime)
    //{
    //    float tSkill_0_CurCoolTime = 0;

    //    while(tMaxCoolTime >= tSkill_0_CurCoolTime)
    //    {
    //        _Skill_0_CoolTimeUI.fillAmount = 1 - tSkill_0_CurCoolTime / tMaxCoolTime;
    //        tSkill_0_CurCoolTime += Time.deltaTime;
    //        yield return null;
    //    }
        
    //}

    /* tMaxDuration = 0은 무한 지속 */
    public void Skill(float tMaxCoolTime, float tMaxDurationTime, int tSkillNum)
    {

        if (tMaxDurationTime != 0)
        {
            // 지속 시간 UI
            StartCoroutine(SkillDurationTime(tMaxDurationTime, tSkillNum));
        }
        else
        {
            // 지속 시간 UI(무한)
            SkillDurationTimeInfinityOn(tSkillNum);
        }

        // 쿨타임 계산, 쿨타임 UI
        StartCoroutine(SkillCoolTime(tMaxCoolTime, tSkillNum));
    }

    IEnumerator SkillDurationTime(float tMaxDurationTime, int tSkillNum)
    {
        Image tSkillDurationTimeUI = _Skill_DurationTimeUIs[tSkillNum];
        float tSkillCurDurationTime = 0;

        while (tMaxDurationTime >= tSkillCurDurationTime)
        {
            tSkillDurationTimeUI.fillAmount = 1 - (tSkillCurDurationTime / tMaxDurationTime);
            tSkillCurDurationTime += Time.deltaTime;
            yield return null;
        }

        tSkillDurationTimeUI.fillAmount = 0;
    }

    public void SkillDurationTimeInfinityOn(int tSkillNum)
    {
        Image tSkillDurationTimeUI = _Skill_DurationTimeUIs[tSkillNum];

        tSkillDurationTimeUI.fillAmount = 1;
    }

    public void SkillDurationTimeInfinityOff(int tSkillNum)
    {
        Image tSkillDurationTimeUI = _Skill_DurationTimeUIs[tSkillNum];

        tSkillDurationTimeUI.fillAmount = 0;
    }


    IEnumerator SkillCoolTime(float tMaxCoolTime, int tSkillNum)
    {
        Image tSkillCoolTimeUI = _Skill_CoolTimeUIs[tSkillNum];
        float tSkillCurCoolTime = 0;

        while (tMaxCoolTime >= tSkillCurCoolTime)
        {
            tSkillCoolTimeUI.fillAmount = 1 - tSkillCurCoolTime / tMaxCoolTime;
            tSkillCurCoolTime += Time.deltaTime;
            yield return null;
        }
    }
}
