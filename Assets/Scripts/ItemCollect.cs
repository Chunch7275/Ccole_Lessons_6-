using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class ItemCollect : NetworkBehaviour
{
    public delegate void CollectItem(Item.VegetableType item);

    public static event CollectItem ItemCollected;
    
    private Dictionary<Item.VegetableType, int> inventory = new Dictionary<Item.VegetableType, int>();

    private Collider LastCollidedItem;

    void Start()
    {
        foreach (Item.VegetableType type in System.Enum.GetValues(typeof(Item.VegetableType)))
        {
            inventory.Add(type, 0);
        }
    }

    void Update()
    {
        if (LastCollidedItem != null && Input.GetKeyDown(KeyCode.Space))
        {
            Item item = LastCollidedItem.GetComponent<Item>();
            AddItemToInventory(item);
            ItemCollected?.Invoke(item.veggieType);
            PrintInventory();
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if (collider.CompareTag("Vegetable"))
        {
            LastCollidedItem = collider;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if (collider.CompareTag("Vegetable"))
        {
            LastCollidedItem = null;
        }
    }

    private void AddItemToInventory(Item item)
    {
        inventory[item.veggieType]++;
    }

    private void PrintInventory()
    {
        string output = "";

        foreach (KeyValuePair<Item.VegetableType, int> pair in inventory)
        {
            output += string.Format("{0}: {1}; ", pair.Key, pair.Value);
        }

        Debug.Log(output);
    }
    // Update is called once per frame
}
