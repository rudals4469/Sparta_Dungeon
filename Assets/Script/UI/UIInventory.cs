using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Selected Item")]

    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;
    
    private int _curEquipIndex;
    
    private ItemData _selectedItem;
    private int _selectedItemIndex;

    private PlayerController _controller;
    private PlayerCondition _condition;

    private void Start()
    {
        _controller = CharacterManager.Instance.Player.controller;
        _condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        _controller.Inventory += Toggle;
        CharacterManager.Instance.Player.AddItem += AddItem;
        
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;
        
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        
        // 아이템이 중복 가능 한지 (CanStack이 true인지)
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        
        // 비어있는 슬롯을 가져온다.

        ItemSlot emptySlot = GetEmptySlot();
        
        // 비어있는 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpateUI();
            CharacterManager.Instance.Player.itemData = null;
        }
        // 없다면
        else
        {
           
            ThrowItem(data);
        }
       
        CharacterManager.Instance.Player.itemData = null;
    }

    void UpateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void ThrowItem(ItemData data)
    {
        GameObject droppedItem = Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));

        // 레이어를 Interactable로 설정
        droppedItem.layer = LayerMask.NameToLayer("Interactable");
    }

    public void SelectedItem(int index)
    {
        if (slots[index].item == null) return;

        _selectedItem = slots[index].item;
        _selectedItemIndex = index;

        selectedItemName.text = _selectedItem.displayName;
        selectedItemDescription.text = _selectedItem.description;
        
        selectedItemStatName.text = String.Empty;
        selectedItemStatValue.text = String.Empty;

        for (int i = 0; i < _selectedItem.consumables.Length; i++)
        {
            selectedItemStatName.text += _selectedItem.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += _selectedItem.consumables[i].value + "\n";
        }
        
        useButton.SetActive(_selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(_selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(_selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (_selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < _selectedItem.consumables.Length; i++)
            {
                switch (_selectedItem.consumables[i].type)
                {
                   case ConsumableType.Health:
                       _condition.Heal(_selectedItem.consumables[i].value);
                       break;
                   case ConsumableType.Hunger:
                       _condition.Eat(_selectedItem.consumables[i].value);
                       break;
                   case ConsumableType.Boost:
                       _controller.BoostMoveSpeed(_selectedItem.consumables[i].value, _selectedItem.effectDuration);
                       break;
                   case ConsumableType.DoubleJump:
                       _controller.EnableDoubleJump(true, _selectedItem.effectDuration);
                       break;
                }
            }
            
            RemovedSelectedItem();
        }
    }

    public void OnDropButton()
    {
        if (_selectedItem != null && _selectedItem.type == ItemType.Equipable && slots[_selectedItemIndex].equipped)
        {
            EquipmentManager.Instance.Unequip(_selectedItem);
            slots[_selectedItemIndex].equipped = false;
        }
        
        ThrowItem(_selectedItem);
        RemovedSelectedItem();
    }

    void RemovedSelectedItem()
    {
        slots[_selectedItemIndex].quantity--;

        if (slots[_selectedItemIndex].quantity <= 0)
        {
            _selectedItem = null;
            slots[_selectedItemIndex].item = null;
            _selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        
        UpateUI();
    }
    
    public void OnEquipButton()
    {
        if (_selectedItem != null && _selectedItem.type == ItemType.Equipable)
        {
            EquipmentManager.Instance.Equip(_selectedItem);
            slots[_selectedItemIndex].equipped = true;
            SelectedItem(_selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        if (_selectedItem != null && _selectedItem.type == ItemType.Equipable)
        {
            EquipmentManager.Instance.Unequip(_selectedItem);
            slots[_selectedItemIndex].equipped = false;
            SelectedItem(_selectedItemIndex);
        }
    }
    
}
