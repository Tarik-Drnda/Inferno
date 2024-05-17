using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;

    private float vrijeme = 0f;
    public string GetItemName()
    {
        return ItemName;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject==gameObject)
        {
            //if invenory is not full
            if (InventorySystem.Instance.CheckSlotsAvailable() )
            {
                
             InventorySystem.Instance.AddToInventory(ItemName);
             InventorySystem.Instance.itemsPickedup.Add(gameObject.name);
             Destroy(gameObject);
             
            }
            else
            {
                Debug.Log("The inventory is full!");
            }
        }
    }
   
}