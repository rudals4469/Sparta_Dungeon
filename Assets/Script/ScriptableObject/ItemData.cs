using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
    Hunger,
    Boost,
    DoubleJump
}

public enum EquipEffectType
{
    None,
    MoveSpeed,
    JumpPower,
}

[System.Serializable]
public class EquipEffect
{
    public EquipEffectType  effectType;
    public float value;
}
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
    
    [Header("Effect")]
    public float effectDuration; // 0이면 즉시형, 0이상이면 버프형
    
    [Header("Equip effect")]
    public EquipEffect[] equipEffects;

}
