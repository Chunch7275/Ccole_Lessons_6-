using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        ItemCollect.ItemCollected += IncrementItem;
    }

    // Update is called once per frame
    void Update()
    {
    }
        
    void IncrementItem(Item.VegetableType veggieType)
    {
        CountGui count = items[(int)veggieType].GetComponent<CountGui>();
        count.UpdateCountBroadcast();
    }
}
