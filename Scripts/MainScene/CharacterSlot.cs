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
        // �÷��̾� ������ ���ӸŴ����� �ѱ�� ���� ����
        GameManager._Inst._PlayerStatus = _PlayerStatus;

        // ����
        _MainSounds.MouseClickSound();

        _FadeController.FadeOutWithChangeScene("GameScene");
    }

    public void UpdateSlot()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("CharacterSprite"))
            {
                // ĳ���� �̹��� ��������Ʈ
                transform.GetChild(i).GetComponent<Image>().sprite = _PlayerStatus.GetPlayerSprite;
            }
            else if (transform.GetChild(i).CompareTag("CharacterName"))
            {
                // ĳ���� �̸�
                transform.GetChild(i).GetComponent<TMP_Text>().text = _PlayerStatus.GetPlayerName;
            }
            else if (transform.GetChild(i).CompareTag("CharacterInfo"))
            {
                // ĳ���� ����
                transform.GetChild(i).GetComponent<TMP_Text>().text = _PlayerStatus.GetPlayerCharacterInfo;
            }
        }
    }
}
