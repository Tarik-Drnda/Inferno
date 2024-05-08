using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    
    //Player health

    public float currentHealth;
    public float maxHealth;
    
    //player Calories 
    public float currentCalories;
    public float maxCalories;

    private float distanceTraveled = 0;

    private Vector3 lastPosition;
    public GameObject playerBody;    
    
    //player hydration
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    public bool isHydrationActive;
    public static PlayerState Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        StartCoroutine(decreaseHydration());

    }

    IEnumerator decreaseHydration()
    {
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(2);
        }
    }

    // Update is called once per frame
    void Update()
    {

        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTraveled >= 5)
        {
            distanceTraveled = 0;
            currentCalories -= 1;
        }
        
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }


    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    public void setHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }
}