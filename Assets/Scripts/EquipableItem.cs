using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;

    private GameObject playerObject;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerObject = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && InventorySystem.Instance.isOpen==false && SelectionManager.Instance.pointerIsVisible==false && SelectionManager.Instance.inInteraction==false)
        { 
            animator.SetTrigger("hit");
            StartCoroutine(PlaySoundDuringAnimation("hit"));
            
        }
        
    }
    private IEnumerator PlaySoundDuringAnimation(string animationTrigger)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.swingSound);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        if (SoundManager.Instance.swingSound.isPlaying)
        {
            SoundManager.Instance.swingSound.Stop();
        }
    }
}
