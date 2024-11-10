using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] [Range(8, 64)] private int itemsToGenerate = 10;

    [Header("UI")] [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject itemUIElementPrefab;

    [Header("Search & Sort")] [SerializeField]
    private int idToSearch = 6;

    [SerializeField] private string nameToSearch = "Item 6";

    private List<ItemUIElement> _itemUIElements;


    private void Awake()
    {
        // Initialize the array
        InitializeArray();

        // Create the UI elements
        CreateUIElements();
    }

    private void InitializeArray()
    {
        if (inventoryItems == null)
            inventoryItems = new List<InventoryItem>();

        // Generate random items
        for (var i = 0; i < itemsToGenerate; i++)
            inventoryItems.Add(InventoryItem.CreateRandom());
    }

    private InventoryItem LinearSearchByName(string name)
    {
        for (var i = 0; i < inventoryItems.Count; i++)
        {
            var item = inventoryItems[i];

            if (item.Name != name)
                continue;

            Debug.Log($"Item found: {item} at index {i}");
            return item;
        }

        return null;
    }

    private InventoryItem BinarySearchByID(int id)
    {
        // Create a sorted array of the inventory items sorted by ID
        var sortedItems = inventoryItems.ToArray();
        Array.Sort(sortedItems, (item1, item2) => item1.ID.CompareTo(item2.ID));

        // Replace the inventory items with the sorted items
        inventoryItems = new List<InventoryItem>(sortedItems);

        // Perform the binary search
        var item = BinarySearchByID(sortedItems, id, 0, sortedItems.Length - 1);

        // Create a new list of item UI elements
        CreateUIElements();

        return item;
    }

    private InventoryItem BinarySearchByID(InventoryItem[] items, int id, int leftInclusive, int rightInclusive)
    {
        // Break if the left index is greater than the right index / No items left to search
        if (leftInclusive > rightInclusive)
            return null;

        // Get the midpoint of the array
        var middle = (leftInclusive + rightInclusive) / 2;
        var middleItem = items[middle];

        // Check if the middle item is the item we are looking for
        if (middleItem.ID == id)
        {
            Debug.Log($"Item found: {middleItem} at index {middle}");
            return middleItem;
        }

        // Look in the right half of the array
        if (middleItem.ID < id)
            return BinarySearchByID(items, id, middle + 1, rightInclusive);

        // Look in the left half of the array
        return BinarySearchByID(items, id, leftInclusive, middle - 1);
    }

    private InventoryItem[] QuickSortByValue(InventoryItem[] array, int low, int high)
    {
        // If the array is empty or contains only one item, return the array
        if (low >= high)
            return array;

        // Create a copy of the array
        var newArray = array.Clone() as InventoryItem[];

        // Get the pivot index
        var pivotIndex = Partition(newArray, low, high);

        // Recursively sort the left and right halves of the array
        QuickSortByValue(newArray, low, pivotIndex - 1);
        QuickSortByValue(newArray, pivotIndex + 1, high);

        return newArray;
    }

    private void Swap(InventoryItem[] array, int index1, int index2)
    {
        var temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }

    private int Partition(InventoryItem[] array, int low, int high)
    {
        // Get the pivot value
        var pivot = array[high].Value;

        var i = low - 1;

        // Iterate through the array
        // If the current value is less than the pivot value, swap it with the value at index i
        for (var j = low; j < high; j++)
        {
            if (array[j].Value < pivot)
            {
                i++;
                Swap(array, i, j);
            }
        }

        // Move the pivot value to the correct position
        Swap(array, i + 1, high);

        return i + 1;
    }

    private void CreateUIElements()
    {
        // If the list is not null, destroy all the elements
        if (_itemUIElements != null)
            foreach (var element in _itemUIElements)
                Destroy(element.gameObject);

        // Create a new list of item UI elements
        _itemUIElements = new List<ItemUIElement>();

        // Get the width and height of the item UI element prefab
        var prefabRectTransform = itemUIElementPrefab.GetComponent<RectTransform>();
        var prefabWidth = prefabRectTransform.rect.width;
        var prefabHeight = prefabRectTransform.rect.height;

        // Get the width and height of the canvas
        var canvasRectTransform = canvas.GetComponent<RectTransform>();
        var canvasWidth = canvasRectTransform.rect.width;
        var canvasHeight = canvasRectTransform.rect.height;

        // Calculate the number of columns and rows
        var columns = (int)(canvasWidth / prefabWidth);
        var rows = (int)(canvasHeight / prefabHeight);

        // Calculate the padding between the elements
        var paddingX = (canvasWidth - columns * prefabWidth) / (columns + 1);
        var paddingY = (canvasHeight - rows * prefabHeight) / (rows + 1);

        // Calculate the starting position
        var startPos = new Vector2(paddingX, -paddingY);

        // Create the UI elements
        for (var i = 0; i < inventoryItems.Count; i++)
        {
            var item = inventoryItems[i];

            // Calculate the position of the element
            var x = i % columns;
            var y = i / columns;

            var pos = startPos + new Vector2(
                x * (prefabWidth + paddingX),
                -y * (prefabHeight + paddingY)
            );

            // Instantiate the item UI element
            var itemUIElement = Instantiate(itemUIElementPrefab, pos, Quaternion.identity, canvas.transform)
                .GetComponent<ItemUIElement>();

            // Initialize the item UI element
            itemUIElement.Initialize(item, pos);

            // Add the item UI element to the list
            _itemUIElements.Add(itemUIElement);
        }
    }

    #region Button Functions

    public void SearchForID()
    {
        BinarySearchByID(idToSearch);
    }

    public void SearchForName()
    {
        LinearSearchByName(nameToSearch);
    }

    public void QuickSort()
    {
        // Replace the inventory items with the sorted items
        inventoryItems =
            new List<InventoryItem>(QuickSortByValue(inventoryItems.ToArray(), 0, inventoryItems.Count - 1));

        // Create new UI elements
        CreateUIElements();
    }

    #endregion
}