using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f; // Adjust the speed of rotation
    public float attackRange = 40.0f;
    public float attackDamage = 10f;
    public float attackCooldown = 10f;

    private Transform target;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    public GameObject playerState;
    private Animator animator;
    
    private Rigidbody rb;
    public float distance = 10f;

    private Vector3 startPoint; // Početna tačka kretanja
    private Vector3 endPoint; // Krajnja tačka kretanja
    private bool movingForward = true;
    void Start()
    {
        animator = GetComponent<Animator>();
        // Find the player GameObject and set it as the target
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Get or add Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.constraints = RigidbodyConstraints.FreezePositionY; // Lock Y-axis position
        rb.isKinematic = true; // Ugasi fiziku za sada
        startPoint = transform.position; // Postavi početnu tačku na trenutnu poziciju prefaba
        endPoint = startPoint + transform.forward * distance;
    }

    void Update()
    {
        // Da li igrac postoji
        if (target != null)
        {
            // Iskakulisi mi udaljenost do igraca
            float distanceToTarget = Vector3.Distance(transform.position,target.position);
            Debug.Log(distanceToTarget);

            // Check if player is within attack range
            if (distanceToTarget <= attackRange)
            {
                RotateTowardsPlayer();
                MoveTowardsPlayer();
                animator.Play("moving");
                // Check if enough time has passed since last attack
                Debug.Log(Time.time);
                Debug.Log("Attack time: " + lastAttackTime + attackCooldown);
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack();
                }
                else
                {

                    isAttacking = false;
                }

            }
            else
            {
                Vector3 targetPosition = movingForward ? endPoint : startPoint;

                // Pomjeri prefab ka ciljnoj poziciji
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Ako prefab stigne do ciljne pozicije, promijeni smjer kretanja
                if (transform.position == targetPosition)
                {
                    movingForward = !movingForward;
                }

            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 enemy = this.transform.position;
        Vector3 targetPlayer = target.position;

        Vector3 newPosition = Vector3.MoveTowards(enemy, targetPlayer, moveSpeed * Time.deltaTime);

        this.transform.position = new Vector3(newPosition.x,0f,newPosition.z);
    }

    void RotateTowardsPlayer()
    {
    // Izračunaj smjer prema igraču
    Vector3 direction = Vector3.RotateTowards(this.transform.position*45, target.transform.position,
        rotationSpeed * Time.deltaTime *Mathf.Deg2Rad , 0f);
        // Rotiraj prema igraču samo na X i Z osima
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

    }

    void Attack()
    {
        playerState.GetComponent<PlayerState>().currentHealth -= 10;
        lastAttackTime = Time.time;
        isAttacking = true;
    }
}
