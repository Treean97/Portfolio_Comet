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
                // 아이템 이미지 스프라이트
                transform.GetChild(i).GetComponent<Image>().sprite = _ItemStatus.GetItemSprite;
            }
            else if (transform.GetChild(i).CompareTag("ItemSelectName"))
            {
                // 아이템 이름
                transform.GetChild(i).GetComponent<TMP_Text>().text = _ItemStatus.GetItemName;
            }
            else if (transform.GetChild(i).CompareTag("ItemSelectInfo"))
            {
                // 아이템 설명
                transform.GetChild(i).GetComponent<TMP_Text>().text = _ItemStatus.GetItemInfo;
            }
        }
    }

    // 아이템 선택 버튼
    public void OnSelectItemBtn()
    {
        // 부모가 가지고있는 Items 스크립트에서 아이템 정보를 가져와서 플레이어에게 전달
        _Player.AddItem(_ItemStatus);

        // UI Off
        _GameSceneUI.SelectItemUIOnOff();
    }
}
