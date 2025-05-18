using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class SpawnManager : NetworkBehaviour
{
    // Start is called before the first frame update

    public GameObject[] lilyPads;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        InvokeRepeating("SpawnLilyPad", 2.0f, 5.0f);
    }

    // Update is called once per frame

    void SpawnLilyPad()
    {
        foreach (GameObject lilyPad in lilyPads)
        {
            NetworkObject lilyPadObject = Instantiate(lilyPad).GetComponent<NetworkObject>();

            lilyPadObject.Spawn();
        }
    }
    
    void Update()
    {
        
    }
}
