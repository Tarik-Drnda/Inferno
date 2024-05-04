using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }
    public bool onTarget=false;
    
    public GameObject interaction_Info_UI;
    Text interaction_text;
    public GameObject hud;
    public GameObject item;
 
    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
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
        if (Physics.Raycast (ray, out hit))
        {
            var selectionTransform = hit.transform;
            item = hit.transform.gameObject;
            if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().playerInRange)
            {
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
                hud.SetActive(true);
                onTarget = true;
             
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                hud.SetActive(false);
            }
 
        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            hud.SetActive(false);
        }
    }
}
