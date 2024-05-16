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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && InventorySystem.Instance.isOpen==false && SelectionManager.Instance.pointerIsVisible==false && SelectionManager.Instance.inInteraction==false)
        {
            animator.SetTrigger("hit");
            SoundManager.Instance.PlaySound(SoundManager.Instance.swingSound);
            
        }

        if (Input.GetKey(KeyCode.Mouse1) && InventorySystem.Instance.isOpen == false &&
            SelectionManager.Instance.pointerIsVisible == false && SelectionManager.Instance.inInteraction == false)
        {
            if (this.GetComponent<InteractableObject>().GetItemName() == "Health potion")
            {
                PlayerState.Instance.currentHealth += 25;
                Destroy(this.transform.gameObject);
            }
        }
    }
    
}
