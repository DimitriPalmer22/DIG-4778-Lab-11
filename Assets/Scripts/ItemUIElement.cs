using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUIElement : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text valueText;

    private RectTransform _rectTransform;

    private InventoryItem _item;

    private void Awake()
    {
        // Get the RectTransform component
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(InventoryItem item, Vector2 pos)
    {
        _item = item;

        // Set the position of the item
        _rectTransform.anchoredPosition = pos;

        // Set the text of the item
        SetText();
    }

    private void SetText()
    {
        nameText.text = _item.Name;
        idText.text = $"ID: {_item.ID.ToString().PadLeft(8, '0')}";
        valueText.text = $"${_item.Value:0.00}";
    }
}