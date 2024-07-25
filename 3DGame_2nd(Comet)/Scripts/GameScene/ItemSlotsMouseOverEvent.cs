using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemSlotsMouseOverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // �ش� ������ ����
    ItemStatus _ItemStatus;

    // ui����â
    [SerializeField]
    GameObject _ItemInfoUIGO;

    [SerializeField]
    TextMeshProUGUI _ItemName;

    [SerializeField]
    TextMeshProUGUI _ItemDesc;

    float _ItemInfoUIWidth;
    float _ItemInfoUIHeight;

    bool IsMouseOver = false;

    // Start is called before the first frame update
    void Start()
    {
        _ItemInfoUIWidth = _ItemInfoUIGO.GetComponent<RectTransform>().rect.width;
        _ItemInfoUIHeight = _ItemInfoUIGO.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMouseOver)
        {
            // ���콺 ��ġ�� �̵�
            //_ItemInfoUIGO.transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            _ItemInfoUIGO.transform.position =
                new Vector2(Input.mousePosition.x + (_ItemInfoUIWidth / 2), Input.mousePosition.y - (_ItemInfoUIHeight / 2));
        }
    }

    public void GetItemSlotInfo(ItemStatus tItemStatus)
    {
        _ItemStatus = tItemStatus;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���콺 ���� �� UI ���� ���
        _ItemInfoUIGO.SetActive(true);

        IsMouseOver = true;

        // ������ �̸� ����
        _ItemName.text = _ItemStatus.GetItemName;

        // ������ ���� ����
        _ItemDesc.text = _ItemStatus.GetItemInfo;        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOver = false; 

        // UI ����â off
        _ItemInfoUIGO.SetActive(false);
    }
}
