using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    private Rigidbody ballRigidBody;
    void Start()
    {
        ballRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ballRigidBody.velocity = ballRigidBody.velocity.normalized * 10;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
    }
    private void OnCollisionExit(Collision other)
    {
        Debug.Log($"EXIT ->{other.gameObject.name}");
    }
    
    private void OnCollisionStay(Collision other)
    {
        Debug.Log($"STAY -> {other.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name);
    }

    void Update()
    {
        
    }
}
