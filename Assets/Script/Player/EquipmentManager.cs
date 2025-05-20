using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    // 여러 장비를 리스트로 관리
    public List<ItemData> equippedItems = new List<ItemData>();

    private PlayerController _player;

    private void Awake()
    {
        Instance = this;
        _player = FindObjectOfType<PlayerController>();
    }

    public void Equip(ItemData item)
    {
        if (!equippedItems.Contains(item))
        {
            equippedItems.Add(item);
            _player.ApplyEquipEffects(item);
        }
    }

    public void Unequip(ItemData item)
    {
        if (equippedItems.Contains(item))
        {
            _player.RemoveEquipEffects(item);
            equippedItems.Remove(item);
        }
    }
}
