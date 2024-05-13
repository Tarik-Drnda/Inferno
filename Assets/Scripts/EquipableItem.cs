using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;

    private GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            Debug.Log("ima playara");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && InventorySystem.Instance.isOpen==false && playerObject.GetComponent<SelectionManager>().pointerIsVisible==false)
        {
            Debug.Log(playerObject.GetComponent<SelectionManager>().pointerIsVisible);
            animator.SetTrigger("hit");
            
        }
    }
}
