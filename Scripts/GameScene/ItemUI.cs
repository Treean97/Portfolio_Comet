using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    Image _ItemIcon;

    [SerializeField]
    TMP_Text _ItemName;

    [SerializeField]
    TMP_Text _ItemInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateItemUI(ItemStatus tItemStatus)
    {
        // todo
        // �������� tItemStatus�� ��������Ʈ �߰�
        _ItemIcon.sprite = tItemStatus.GetItemSprite;
        _ItemName.text = tItemStatus.GetItemName;
        _ItemInfo.text = tItemStatus.GetItemInfo;
    }

}
