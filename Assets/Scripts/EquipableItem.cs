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
            StartCoroutine(PlaySoundDuringAnimation("hit"));
            
        }
        
    }
    private IEnumerator PlaySoundDuringAnimation(string animationTrigger)
    {
        // Play the sound
        SoundManager.Instance.PlaySound(SoundManager.Instance.swingSound);

        // Get the animation clip length
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Wait for the animation to complete
        yield return new WaitForSeconds(animationLength);

        // Stop the sound (if it is still playing)
        if (SoundManager.Instance.swingSound.isPlaying)
        {
            SoundManager.Instance.swingSound.Stop();
        }
    }
}
