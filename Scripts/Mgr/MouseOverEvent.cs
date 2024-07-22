using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseEvent : MonoBehaviour
    , IPointerClickHandler
    , IDragHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{

    [SerializeField]
    bool _IsScaleChangeEffect;

    [Range(1f, 2f)]
    [SerializeField]
    float _ScaleAmount;

    [SerializeField]
    MainSounds _MainSounds;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        // ����        
        _MainSounds.MouseClickSound();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // ����
        _MainSounds.MouseOverSound();

        // ȿ��
        if (_IsScaleChangeEffect)
        {
            gameObject.GetComponent<RectTransform>().localScale = Vector3.one * _ScaleAmount;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        // ȿ��
        if (_IsScaleChangeEffect)
        {
            gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        
    }



}
