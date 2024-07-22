using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSceneUIMouseEvent : MonoBehaviour
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
    GameSounds _GameSounds;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        // 사운드        
        _GameSounds.MouseClickSound();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // 사운드
        _GameSounds.MouseOverSound();

        // 효과
        if (_IsScaleChangeEffect)
        {
            gameObject.GetComponent<RectTransform>().localScale = Vector3.one * _ScaleAmount;
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        // 효과
        if (_IsScaleChangeEffect)
        {
            gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {

    }



}
