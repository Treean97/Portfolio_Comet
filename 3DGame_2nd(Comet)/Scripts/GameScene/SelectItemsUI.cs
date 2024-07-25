using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using TMPro.EditorUtilities;
#endif

public class SelectItemsUI : MonoBehaviour
{
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
        UpdateItemList();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");

            // �پ�
            if (wheelInput > 0)
            {
                _SelectItemScrollUI.value += _ScrollSpeed;
                                
            }
            // �ٴٿ�
            else if (wheelInput < 0)
            {
                _SelectItemScrollUI.value -= _ScrollSpeed;

            }
        }
    }

    // ������ ��� ����
    void UpdateItemList()
    {
        // ������ ������ ��ü ������ ����Ʈ���� �ϳ��� ������
        foreach(var tStatus in _ItemDictionary.GetTotalItemStatus)
        {
            // ������ ���� �������� ���� �������� �������ִ� ��ũ��Ʈ�� ������ ������ �־���
            GameObject tSelectItemSlotGO = Instantiate<GameObject>(_SelectItemSlot, _Contents);
            SelectItemsSlot tSelectItemSlot = tSelectItemSlotGO.GetComponent<SelectItemsSlot>();
            tSelectItemSlot._ItemStatus = tStatus;
            tSelectItemSlot.UpdateItemList();

        }
    }


}
