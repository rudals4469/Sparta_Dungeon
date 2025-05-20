using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline _outline;

    public UIInventory inventory;
    public int index;

    private bool _equipped;
    public bool equipped
    {
        get => _equipped;
        set
        {
            _equipped = value;
            UpdateOutline();
        }
    }

    public int quantity;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        UpdateOutline();
    }

    public void Set()
    {
        if (item == null)
        {
            Clear();
            return;
        }

        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        UpdateOutline();
    }

    public void Clear()
    {
        item = null;
        equipped = false;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;

        UpdateOutline();
    }

    public void OnClickButton()
    {
        inventory.SelectedItem(index);
    }

    private void UpdateOutline()
    {
        if (_outline != null)
        {
            _outline.enabled = equipped;
        }
    }
}