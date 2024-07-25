using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemStatus", menuName = "Scriptable Object/ItemStatus", order = int.MaxValue - 3)]
public class ItemStatus : ScriptableObject
{
    // 아이템 id
    [SerializeField]
    int ItemId = 0;
    public int GetItemId { get { return ItemId; } }

    // 아이템 이름
    [SerializeField]
    string ItemName;
    public string GetItemName { get { return ItemName; } }

    [SerializeField]
    Sprite ItemSprite;
    public Sprite GetItemSprite { get { return ItemSprite; } }

    // 아이템 타입
    public enum ItemType
    {
        Status,
        Passive
    }
    [SerializeField]
    ItemType _ItemType;

    public ItemType GetItemType { get { return _ItemType; } }

    // 아이템이 올려줄 상승 스탯
    public enum ItemAdvType
    { 
        DamageMultiply = 0,
        MaxJumpCount,
        JumpPower,
        Speed,
        Critical,
        None
    };
    [SerializeField]
    ItemAdvType _ItemAdvType;

    public ItemAdvType GetItemAdvType { get { return _ItemAdvType; } }

    // 아이템 연산 방식

    public enum ItemAdvCalType
    {
        Add = 0,
        Multiply = 1,
        None
    };

    [SerializeField]
    ItemAdvCalType _ItemAdvCalType;

    public ItemAdvCalType GetItemAdvCalType { get { return _ItemAdvCalType; } }

    // 연산 수치
    [SerializeField]
    float ItemAdvValue;
    public float GetItemAdvValue { get { return ItemAdvValue; } }

    [SerializeField]
    float _PassiveChance;
    public float _GetPassiveChance { get { return _PassiveChance; } }

    [SerializeField]
    string ItemInfo;
    public string GetItemInfo { get { return ItemInfo; } }
}
