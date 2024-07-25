using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotsUI : MonoBehaviour
{
    // 플레이어의 인벤토리 정보를 받아오기 위함
    [SerializeField]
    Player _Player;

    // 아이템 전체 정보
    [SerializeField]
    ItemDictionary _ItemDictionary;

    [SerializeField]
    GameObject[] _ItemSlotUIGOs;

    int _SlotIndex;

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // 최초 실행 시 Player 검색
        if (_Player == null)
        {
            _Player = FindObjectOfType<Player>();
        }

        

        if (_Player.GetPlayerInventory != null)
        {
            _SlotIndex = 0;
            ClearSlots();

            foreach (KeyValuePair<int, int> t in _Player.GetPlayerInventory)
            {
                // 인벤토리 n번 째 아이템의 id : t.Key
                // 인벤토리 n번 째 아이템의 갯수 : t.Value
                
                // 아이템 목록에서 id값을 검색해서 해당 아이템 슬롯 정보 갱신
                UpdateAllItemSlots(t.Key, t.Value);
            }
        }

    }

    void UpdateAllItemSlots(int tId, int tCount)
    {
        foreach (var tStatus in _ItemDictionary.GetTotalItemStatus)
        {
            // 같은 Id를 찾으면 아이템 슬롯UI를 하나 생성하고 해당 이미지를 아이템슬롯에 할당
            if(tId == tStatus.GetItemId)
            {
                UpdateSlot(tStatus, tCount);
                ActiveSlot();

                // 해당 아이템 정보를 넘겨줌
                _ItemSlotUIGOs[_SlotIndex].GetComponentInChildren<ItemSlotsMouseOverEvent>().GetItemSlotInfo(tStatus);

                _SlotIndex++;
                break;
            }
        }

    }

    void ClearSlots()
    {
        // 모든 슬롯 비활성화
        foreach(var tSlot in _ItemSlotUIGOs)
        {
            tSlot.SetActive(false);
        }
    }

    void ActiveSlot()
    {
        // 슬롯 활성화
        _ItemSlotUIGOs[_SlotIndex].SetActive(true);
    }

    void UpdateSlot(ItemStatus tStatus, int tCount)
    {
        // 아이템 UI 이미지 설정
        _ItemSlotUIGOs[_SlotIndex].GetComponentInChildren<Image>().sprite = tStatus.GetItemSprite;

        // 아이템 수량 설정
        _ItemSlotUIGOs[_SlotIndex].GetComponentInChildren<TMP_Text>().text = tCount.ToString();
    }
}
