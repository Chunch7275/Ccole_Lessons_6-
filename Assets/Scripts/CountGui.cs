using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class CountGui : NetworkBehaviour
{
    private TextMeshProUGUI tmProElement;

    public string itemName;

    public NetworkVariable<int> count = new NetworkVariable<int>(0);  

    // Start is called before the first frame update
    void Start()
    {
        tmProElement = GetComponent<TextMeshProUGUI>();
        count.OnValueChanged += OnCountValueChanged;
    }

    public override void OnNetworkSpawn()
    {
        UpdateText();
    }

    public void UpdateCountBroadcast()
    {
        if (IsServer)
        {
            UpdateCount();
        }
        else
        {
            UpdateCountRPC();
        }
    }

    private void OnCountValueChanged(int previousValue, int newValue)
    {
        UpdateText();
    }

    // Update is called once per frame
    public void UpdateCount()
    {
        count.Value ++;
    }

    [Rpc(SendTo.Server)]
    public void UpdateCountRPC()
    {
        UpdateCount();
    }

    public void UpdateText()
    {
        tmProElement.text = itemName + ": " + count.Value;
    }
}