using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotsUI : MonoBehaviour
{
    // �÷��̾��� �κ��丮 ������ �޾ƿ��� ����
    [SerializeField]
    Player _Player;

    // ������ ��ü ����
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
        // ���� ���� �� Player �˻�
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
                // �κ��丮 n�� ° �������� id : t.Key
                // �κ��丮 n�� ° �������� ���� : t.Value
                
                // ������ ��Ͽ��� id���� �˻��ؼ� �ش� ������ ���� ���� ����
                UpdateAllItemSlots(t.Key, t.Value);
            }
        }

    }

    void UpdateAllItemSlots(int tId, int tCount)
    {
        foreach (var tStatus in _ItemDictionary.GetTotalItemStatus)
        {
            // ���� Id�� ã���� ������ ����UI�� �ϳ� �����ϰ� �ش� �̹����� �����۽��Կ� �Ҵ�
            if(tId == tStatus.GetItemId)
            {
                UpdateSlot(tStatus, tCount);
                ActiveSlot();

                // �ش� ������ ������ �Ѱ���
                _ItemSlotUIGOs[_SlotIndex].GetComponentInChildren<ItemSlotsMouseOverEvent>().GetItemSlotInfo(tStatus);

                _SlotIndex++;
                break;
            }
        }

    }

    void ClearSlots()
    {
        // ��� ���� ��Ȱ��ȭ
        foreach(var tSlot in _ItemSlotUIGOs)
        {
            tSlot.SetActive(false);
        }
    }

    void ActiveSlot()
    {
        // ���� Ȱ��ȭ
        _ItemSlotUIGOs[_SlotIndex].SetActive(true);
    }

    void UpdateSlot(ItemStatus tStatus, int tCount)
    {
        // ������ UI �̹��� ����
        _ItemSlotUIGOs[_SlotIndex].GetComponentInChildren<Image>().sprite = tStatus.GetItemSprite;

        // ������ ���� ����
        _ItemSlotUIGOs[_SlotIndex].GetComponentInChildren<TMP_Text>().text = tCount.ToString();
    }
}
