using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    // A counter to keep track of the id of the inventory items
    private static long _idCounter = 0;

    [SerializeField] private long id;
    [SerializeField] private string name;
    [SerializeField] private float value;

    public long ID => id;
    public string Name => name;
    public float Value => value;

    private InventoryItem(string name, float value)
    {
        id = _idCounter;

        this.name = name;
        this.value = value;

        // Increment the id counter
        _idCounter++;
    }

    public static InventoryItem CreateRandom()
    {
        var randomNumber = UnityEngine.Random.Range(0, 100_000_000);
        var randomName = $"Item {randomNumber}";

        var randomValue = UnityEngine.Random.Range(0.00f, 1000.00f);

        return new InventoryItem(randomName, randomValue);
    }
}