using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] [Min(0)] private int itemsToGenerate = 10;

    private void Awake()
    {
        // Initialize the array
        InitializeArray();
    }

    private void InitializeArray()
    {
        if (inventoryItems == null)
            inventoryItems = new List<InventoryItem>();

        // Generate random items
        for (var i = 0; i < itemsToGenerate; i++)
            inventoryItems.Add(InventoryItem.CreateRandom());
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}