using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ItemSlotsMouseOverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 해당 아이템 정보
    ItemStatus _ItemStatus;

    // ui정보창
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
            // 마우스 위치로 이동
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
        // 마우스 오버 시 UI 정보 출력
        _ItemInfoUIGO.SetActive(true);

        IsMouseOver = true;

        // 아이템 이름 설정
        _ItemName.text = _ItemStatus.GetItemName;

        // 아이템 설명 설정
        _ItemDesc.text = _ItemStatus.GetItemInfo;        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOver = false; 

        // UI 정보창 off
        _ItemInfoUIGO.SetActive(false);
    }
}
