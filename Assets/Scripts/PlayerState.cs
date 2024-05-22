using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    

    public float currentHealth;
    public float maxHealth;
    
    public float currentCalories;
    public float maxCalories;

    private float distanceTraveled = 0;

    private Vector3 lastPosition;
    public GameObject playerBody;    
    


    public bool isHydrationActive;
    public static PlayerState Instance { get; set; }

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

   

    void Update()
    {
       
        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (Input.GetKey(KeyCode.LeftShift) && currentCalories>0)
        {
            StartCoroutine(SpendEnergy());
            playerBody.GetComponent<PlayerMovement>().speed = 18f;
        }
        else
        {
            if(currentCalories<100)
                StartCoroutine(RestoreEnergy());
            playerBody.GetComponent<PlayerMovement>().speed = 12f;
        }
        
        if (currentHealth <= 0)
        {
            GameOverManager.Instance.ShowDeathCanvas(); 
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

   public IEnumerator SpendEnergy()
   {
       yield return new WaitForSeconds(1f);
       EnergyConsumption();
   }
   public IEnumerator RestoreEnergy()
   {
       yield return new WaitForSeconds(2f);
       EnergyRegeneration();
   }
}