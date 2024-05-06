using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f; // Adjust the speed of rotation
    public float attackRange = 40.0f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private Transform target;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private Rigidbody rb;
    public float distance = 10f;

    private Vector3 startPoint; // Početna tačka kretanja
    private Vector3 endPoint; // Krajnja tačka kretanja
    private bool movingForward = true;
    void Start()
    {
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
            float distanceToTarget = Vector3.Distance(target.position, transform.position);
            Debug.Log(distanceToTarget);

            // Check if player is within attack range
            if (distanceToTarget <= attackRange)
            {
                MoveTowardsPlayer();
                // Check if enough time has passed since last attack
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    // Attack the player
                    Attack();
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
        //Formula za kalkulisanje vektora prema igracu od prefaba
        Vector3 direction = (transform.position- target.position).normalized;

        // Pomjeri prefab prema igracu novi vektor
        Vector3 movement = new Vector3(direction.x, 0f, direction.z) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void RotateTowardsPlayer()
    {
    // Izračunaj smjer prema igraču
        Vector3 direction = (transform.position- target.position).normalized;
        

        // Rotiraj prema igraču samo na X i Z osima
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, lookRotation.eulerAngles.z);

    }

    void Attack()
    {
        // Perform attack
        Debug.Log("Enemy attacks!");

        // Deal damage to player (You can implement this part)
        // For example, you can access the player's health script and reduce their health

        // Update last attack time
        lastAttackTime = Time.time;
    }
}
