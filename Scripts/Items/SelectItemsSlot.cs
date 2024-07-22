using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SelectItemsSlot : MonoBehaviour
{
    public ItemStatus _ItemStatus;
    Player _Player;
    GameSceneUI _GameSceneUI;

    // Start is called before the first frame update
    void Start()
    {
        _Player = FindObjectOfType<Player>();
        _GameSceneUI = FindObjectOfType<GameSceneUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateItemList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("ItemSelectIcon"))
            {
                // ������ �̹��� ��������Ʈ
                transform.GetChild(i).GetComponent<Image>().sprite = _ItemStatus.GetItemSprite;
            }
            else if (transform.GetChild(i).CompareTag("ItemSelectName"))
            {
                // ������ �̸�
                transform.GetChild(i).GetComponent<TMP_Text>().text = _ItemStatus.GetItemName;
            }
            else if (transform.GetChild(i).CompareTag("ItemSelectInfo"))
            {
                // ������ ����
                transform.GetChild(i).GetComponent<TMP_Text>().text = _ItemStatus.GetItemInfo;
            }
        }
    }

    // ������ ���� ��ư
    public void OnSelectItemBtn()
    {
        // �θ� �������ִ� Items ��ũ��Ʈ���� ������ ������ �����ͼ� �÷��̾�� ����
        _Player.AddItem(_ItemStatus);

        // UI Off
        _GameSceneUI.SelectItemUIOnOff();
    }
}
