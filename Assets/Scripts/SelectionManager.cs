using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }
    public bool onTarget = false;

    public GameObject Crosshair;
    public GameObject Pointer;
    Text interaction_text;

    public GameObject textBox;
    public GameObject hud;
    public GameObject item;

    private bool isInteractableInRange = false;

    public GameObject selectedObject;

  //  public Image centerDotImage; // to je crosshair
    //public Image handIcon; // to je pointer
 

    private void Start()
    {
        interaction_text = textBox.GetComponent<Text>();
        Crosshair.SetActive(true); // Start with UI disabled
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
       

        if (isHit)
        {
            
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            
            item = hit.transform.gameObject;


            if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().playerInRange==true)
            {
                isInteractableInRange = true;
                interaction_text.text = item.GetComponent<InteractableObject>().GetItemName();
                             
                onTarget = true;
                selectedObject = interactable.gameObject;
             
                Pointer.SetActive(true);
                Crosshair.SetActive(false);
                hud.SetActive(true);
                textBox.SetActive(true);
            }
            else
            {
                Pointer.SetActive(false);
                textBox.SetActive(false);
                Crosshair.SetActive(true);
                hud.SetActive(false);
            }
        }
        else
        {
            Crosshair.SetActive(true);
            Pointer.SetActive(false);
            textBox.SetActive(true);
            hud.SetActive(false);
        }



        // Toggle the HUD based on the overall state
        
    }


    public void DisableSelection()
    {
       Pointer.SetActive(false);
       Crosshair.SetActive(false);
        selectedObject = null;
    }

    public void EnableSelection()
    {
        Pointer.SetActive(true);
        selectedObject = null;
    }
}
