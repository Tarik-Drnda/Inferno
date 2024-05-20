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
    
    public GameObject hud;
    public GameObject item;
    public GameObject Interaction_Info_UI;
    private bool isInteractableInRange = false;

    public GameObject selectedObject;

    public bool pointerIsVisible;
    public bool inInteraction;
    
    private void Start()
    {
        interaction_text = Interaction_Info_UI.GetComponent<Text>();
        Crosshair.SetActive(true); 
        Cursor.visible = false;
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
        Cursor.visible = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool isHit = Physics.Raycast(ray, out hit);


        if (isHit)
        {

            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            item = hit.transform.gameObject;
            EnemyAI enemy = selectionTransform.GetComponent<EnemyAI>();
            
            NPC npc = selectionTransform.GetComponent<NPC>();
           SecondNPC nps2 = selectionTransform.GetComponent<SecondNPC>();
           if (nps2 && nps2.playerInRange)
           {
               Interaction_Info_UI.SetActive(true);
               interaction_text.text = "Talk"; 
               hud.SetActive(true); 
               
               if (Input.GetMouseButtonDown(0) && nps2.isTalkingWithPlayer2 == false)
               {
                   nps2.StartConversation();
                   inInteraction = true;
               }
              

               if (DialogSystem.Instance.dialogUIActive)
               {
                   Interaction_Info_UI.SetActive(false);
                   Crosshair.SetActive(false);
                   Cursor.visible = false;

               }
           }

           else if (npc && npc.playerInRange)
            {
                Interaction_Info_UI.SetActive(true);
                interaction_text.text = "Talk"; 
                hud.SetActive(true); 
                if (Input.GetMouseButtonDown(0) && npc.isTalkingWithPlayer == false)
                {
                    npc.StartConversation();
                    inInteraction = true;
                }
               

                if (DialogSystem.Instance.dialogUIActive)
                {
                    Interaction_Info_UI.SetActive(false);
                    Crosshair.SetActive(false);
                    Cursor.visible = false;

                }
            }
            
           else if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().playerInRange==true)
            {
                isInteractableInRange = true;
                interaction_text.text = item.GetComponent<InteractableObject>().GetItemName();
                             
                onTarget = true;
                selectedObject = interactable.gameObject;
                inInteraction = false;

                if (selectedObject.CompareTag("Pickable"))
                {
                    Pointer.SetActive(true);
                    Crosshair.SetActive(false);
                    Interaction_Info_UI.SetActive(true);
                    pointerIsVisible = true;

                }
             
                else
                {
                    Pointer.SetActive(false);
                    Crosshair.SetActive(true);
                    Interaction_Info_UI.SetActive(false);
                    pointerIsVisible = false;

                }
                hud.SetActive(true);
                Interaction_Info_UI.SetActive(true);
             
            }
           
           
            else if (enemy!=null && enemy.playerInRange == true)
            {
                inInteraction = false;
                Interaction_Info_UI.SetActive(true);
                interaction_text.text = enemy.enemyName;
                hud.SetActive(true);
                if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                {
                    StartCoroutine(DealDamageTo(enemy, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
            }
            else
            {
                Pointer.SetActive(false);
                Crosshair.SetActive(true);
                hud.SetActive(false);
                interaction_text.text = "";
                pointerIsVisible = false;
            }

        }
        else
        {
            inInteraction = false;
            onTarget = true;
           Interaction_Info_UI.SetActive(false);
            Crosshair.SetActive(true);
            Pointer.SetActive(false);
            pointerIsVisible = false;
            hud.SetActive(false);
        }
        
    }

    IEnumerator DealDamageTo(EnemyAI enemy, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        enemy.TakeDamage(damage);
        
        
    }


    public void DisableSelection()
    {
       Pointer.SetActive(false);
       Crosshair.SetActive(false);
        selectedObject = null;
        pointerIsVisible = false;
    }

    public void EnableSelection()
    {
        Pointer.SetActive(true);
        pointerIsVisible = true;
        selectedObject = null;
    }
}
