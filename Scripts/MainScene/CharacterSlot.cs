using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterSlot : MonoBehaviour
{
    public PlayerStatus _PlayerStatus;

    [SerializeField]
    MainSounds _MainSounds;

    FadeController _FadeController;

    // Start is called before the first frame update
    void Start()
    {
        _FadeController = GameObject.FindGameObjectWithTag("FadeController").GetComponent<FadeController>();
        _MainSounds = GameObject.FindObjectOfType<MainSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickCharacterSelectBtn()
    {
        // 플레이어 정보를 게임매니저에 넘기고 게임 시작
        GameManager._Inst._PlayerStatus = _PlayerStatus;

        // 사운드
        _MainSounds.MouseClickSound();

        _FadeController.FadeOutWithChangeScene("GameScene");
    }

    public void UpdateSlot()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("CharacterSprite"))
            {
                // 캐릭터 이미지 스프라이트
                transform.GetChild(i).GetComponent<Image>().sprite = _PlayerStatus.GetPlayerSprite;
            }
            else if (transform.GetChild(i).CompareTag("CharacterName"))
            {
                // 캐릭터 이름
                transform.GetChild(i).GetComponent<TMP_Text>().text = _PlayerStatus.GetPlayerName;
            }
            else if (transform.GetChild(i).CompareTag("CharacterInfo"))
            {
                // 캐릭터 설명
                transform.GetChild(i).GetComponent<TMP_Text>().text = _PlayerStatus.GetPlayerCharacterInfo;
            }
        }
    }
}
