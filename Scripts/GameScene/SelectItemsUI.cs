using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemsUI : MonoBehaviour
{
    [SerializeField]
    Player _Player;

    [SerializeField]
    GameSceneUI _GameSceneUI;

    [SerializeField]
    TMP_Text _SelectItemtext;

    [SerializeField]
    Scrollbar _SelectItemScrollUI;

     [SerializeField]
    float _ScrollSpeed;

    [SerializeField]
    GameObject _SelectItemSlot;

    [SerializeField]
    ItemDictionary _ItemDictionary;

    [SerializeField]
    Transform _Contents;

    // Start is called before the first frame update
    void Start()
    {
        _Player = FindObjectOfType<Player>();
        UpdateItemList();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");

            // 휠업
            if (wheelInput > 0)
            {
                _SelectItemScrollUI.value += _ScrollSpeed;
                                
            }
            // 휠다운
            else if (wheelInput < 0)
            {
                _SelectItemScrollUI.value -= _ScrollSpeed;

            }
        }
    }

    // 아이템 목록 갱신
    void UpdateItemList()
    {
        // 아이템 정보를 전체 아이템 리스트에서 하나씩 가져옴
        foreach(var tStatus in _ItemDictionary.GetTotalItemStatus)
        {
            // 아이템 슬롯 프리펩을 만들어서 프리펩이 가지고있는 스크립트에 아이템 정보를 넣어줌
            GameObject tSelectItemSlotGO = Instantiate<GameObject>(_SelectItemSlot, _Contents);
            SelectItemsSlot tSelectItemSlot = tSelectItemSlotGO.GetComponent<SelectItemsSlot>();
            tSelectItemSlot._ItemStatus = tStatus;
            tSelectItemSlot.UpdateItemList();

        }
    }


}
