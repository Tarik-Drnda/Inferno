using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Transform playerTransform;
    private Animator _animator;

    private float distanceToPlayer;
    public float wantedDistance = 0.0f;

    public GameObject bloodScreen;

    private bool canAttack = true; // Flag to control whether the enemy can attack

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
        distanceToPlayer = Vector3.Distance(_agent.transform.position, playerTransform.position);
        Debug.Log(distanceToPlayer);

        if (distanceToPlayer < wantedDistance)
        {
            _agent.destination = playerTransform.position;
            SoundManager.Instance.PlaySound(SoundManager.Instance.walkingSound);

            if (distanceToPlayer <= 3f && canAttack)
            {
                // Start the coroutine to delay the Attack function call
                StartCoroutine(AttackAfterDelay());
                
            }
           
        }
    }

    private IEnumerator AttackAfterDelay()
    {
        // Set canAttack to false to prevent immediate attack
        CancelInvoke("playHit");
        canAttack = false;
       

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Call the Attack function
        Attack();
        
        // Reset canAttack after the attack
        canAttack = true;
    }

    private void Attack()
    {
        Debug.Log("Attacking!");
        // Deal damage to the player
        if (PlayerState.Instance.currentHealth > 0)
        {
            PlayerState.Instance.currentHealth -= 10;
            StartCoroutine(PlayAfterDelay());
            bloodScreen.SetActive(false);
          
        }
        else
        {
            bloodScreen.SetActive(false);
        }
           
        _animator.SetTrigger("isAttacking");
    }

    private IEnumerator PlayAfterDelay()
    {
  
        yield return new WaitForSeconds(1.45f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemyHit);
        bloodScreen.SetActive(true);
    }
}