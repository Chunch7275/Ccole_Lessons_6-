using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rbplayer;

    private Vector3 direction = Vector3.zero;
    [SerializeField]
    private float ForceMultiplier = 10.0f;
    [SerializeField]
    private ForceMode forcemode;

    public GameObject[] spawnPoints;

    

    // Start is called before the first frame update
    void Start()
    {
        rbplayer = GetComponent<Rigidbody>();

        spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        Respawn();
        

    }

    void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        float HorizontalVelocity = Input.GetAxis("Horizontal");
        float VerticalVelocity = Input.GetAxis("Vertical");
        
        direction = new Vector3(HorizontalVelocity, 0, VerticalVelocity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if (IsServer)
        {
            Move(direction);
        } 
        else
        {
            MoveRpc(direction);
        }
        

    }

    private void Move(Vector3 input)
    {
        rbplayer.AddForce(direction * ForceMultiplier, forcemode);

        if (transform.position.z > 38)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 38);
        }
        else if (transform.position.z < -38)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -38);
        }
    }
    [Rpc(SendTo.Server)]
    public void MoveRpc(Vector3 input)
    {
        Move(input);
    }

    private void Respawn()
    {
        int index = 0;
        while (Physics.CheckBox(spawnPoints[index].transform.position, new Vector3 (1.0f, 1.0f, 1.0f)))
        {
            index++;
        }
        rbplayer.MovePosition(spawnPoints[index].transform.position);
    }

    void OnTriggerExit(Collider collider)
    {
        if (!IsServer)
        {
            return;
        }

        if (collider.CompareTag("Hazard"))
        {
            Respawn();
        }
    }
}
