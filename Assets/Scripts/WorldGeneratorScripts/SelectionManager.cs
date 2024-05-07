using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }
    public bool onTarget = false;

    public GameObject interaction_Info_UI;
    Text interaction_text;
    
    
    public GameObject hud;
    public GameObject item;



    public GameObject selectedObject;

    public Image centerDotImage;
    public Image handIcon;
 

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        interaction_Info_UI.SetActive(false); // Start with UI disabled
    }

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

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool isHit = Physics.Raycast(ray, out hit);
        bool isInteractableInRange = false;

        if (isHit)
        {
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            
            item = hit.transform.gameObject;


            if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().playerInRange)
            {
                isInteractableInRange = true;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
          
                onTarget = true;
                selectedObject = interactable.gameObject;
             
                interaction_Info_UI.SetActive(true);
                
                hud.SetActive(true);
                
                


            }
        }

        if (isInteractableInRange)
        {
            // Show UI only if an interactable object is in range
            interaction_Info_UI.SetActive(true);
            onTarget = true;
        }
        else
        {
            // Hide UI if no interactable object is in range
            interaction_Info_UI.SetActive(false);
            onTarget = false;
        }

        // Toggle the HUD based on the overall state
        hud.SetActive(onTarget);
    }


    public void DisableSelection()
    {
        interaction_Info_UI.SetActive(false);
        selectedObject = null;
    }

    public void EnableSelection()
    {
        interaction_Info_UI.SetActive(true);
        selectedObject = null;
    }
}
