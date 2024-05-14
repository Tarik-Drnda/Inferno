using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    


    public bool isHydrationActive;
    public static PlayerState Instance { get; set; }

    // Regeneration
    [CanBeNull] private float timepass = 5f;
    private float timeUkupno = 0f;
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

        
    }

   

    // Update is called once per frame
    void Update()
    {
       
        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (Input.GetKey(KeyCode.LeftShift) && currentCalories>0)
        {

            
            InvokeRepeating("EnergyConsumption", 2f, 1f);

                playerBody.GetComponent<PlayerMovement>().speed = 18f;
                CancelInvoke("EnergyRegeneration");
        }
        else
        {
            CancelInvoke("EnergyConsumption");
            InvokeRepeating("EnergyRegeneration",5,105f);
            playerBody.GetComponent<PlayerMovement>().speed = 12f;
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
        
        if (currentHealth <= 0)
        {
            GameOverManager.Instance.ShowDeathCanvas(); // Pokrenite prikaz canvasa smrti
        }
    }

    public void EnergyRegeneration()
    {
        if(currentCalories!=100)
            currentCalories += 1;
    }

    public void EnergyConsumption()
    {
        if(currentCalories>0)
            currentCalories -= 1;
    }
    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

   
}