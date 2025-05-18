using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavPlayerMovement : MonoBehaviour
{
    public delegate void DropHive(Vector3 pos);

    public static event DropHive DroppedHive;

    public float speed = 6.0f;
    public float rotationSpeed = 60.0f;
    private Rigidbody playerRb = null;
    private float translationValue = 0;
    private float rotateValue = 0;

    private Animator animator;

    public bool dead = false;

    private bool OnAlert = false;

    public GameObject looktarget;

    private Coroutine SmoothLookCoroutine;

    private GameObject currentHazard;
    
    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (dead) {
            return;
        }
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");

        animator.SetFloat("speed", translation);

        translationValue = translation;
        rotateValue = rotation;

        if (Input.GetKeyDown(KeyCode.Space) && HivePickUp.pickedUp && !HivePickUp.dropped)
        {
            DroppedHive.Invoke(transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (dead)
        {
            return;
        }
        // rotates the player
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += rotateValue * rotationSpeed * Time.deltaTime;
        playerRb.MoveRotation(Quaternion.Euler(rot));
        
        // simply moves the player by however much the player is pressing with respect
        // to the speed parameter. Does not affect gravity.
        Vector3 move = transform.forward * translationValue;
        if(translationValue < 0) { 
            move = Vector3.zero;
        }

            playerRb.velocity = new Vector3(move.x * speed, playerRb.velocity.y, move.z * speed);
    }

        void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Hazard") && dead == false)
        {
            dead = true;
            animator.SetTrigger("Died");
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hazard")) 
        {
            if (SmoothLookCoroutine != null)
            {
                StopCoroutine(SmoothLookCoroutine);
            }

            OnAlert = true;
            animator.SetBool("LeftEarStand", OnAlert);
            if (currentHazard == null)
            {
                currentHazard = other.gameObject;
            }
            looktarget.transform.position = Vector3.Lerp(looktarget.transform.position, currentHazard.transform.position, 0.08f);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            currentHazard = null;

            SmoothLookCoroutine = StartCoroutine("SmoothLookForward");
            OnAlert = false;
            animator.SetBool("LeftEarStand", OnAlert);
        }
    }

    IEnumerator SmoothLookForward()
    {
        Vector3 targetposition = transform.position + transform.forward * 6;
        while (Vector3.Distance(looktarget.transform.position, targetposition) > 0.1f)
        {
            targetposition = transform.position + transform.forward * 6;
            looktarget.transform.position = Vector3.Lerp(looktarget.transform.position, targetposition, 0.08f);
            yield return new WaitForSeconds(0.05f);
        }

        looktarget.transform.position = targetposition;
    }

}
