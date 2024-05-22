using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Transform playerTransform;
    private Animator _animator;

    private float distanceToPlayer;
    public float wantedDistance = 0.0f;

    public GameObject bloodScreen;

    private bool canAttack = true; 
    
    public string enemyName;
    public bool playerInRange;
    public bool _isDead=false;
    
    public int currentHealth;
    public int maxHealth;

    private GameObject _bloodGO;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        currentHealth = maxHealth;
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

            if (distanceToPlayer <= 4f && canAttack && _isDead == false) 
            {
                StartCoroutine(AttackAfterDelay());
            }
        }
     
    }

    private IEnumerator AttackAfterDelay()
    {
        CancelInvoke("playHit");
        canAttack = false;
       

        yield return new WaitForSeconds(3f);

        Attack();

        canAttack = true;
    }

    private void Attack()
    {
        Debug.Log("Attacking!");
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
       SoundManager.Instance.PlaySound(SoundManager.Instance.enemySound);
        bloodScreen.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        Destroy(_bloodGO);
        currentHealth -= damage;
       
        if (currentHealth <= 0)
        {
            _animator.SetTrigger("isDead");
            this.transform.position = new Vector3(this.transform.position.x,-1.36f,this.transform.position.z);
            this.GetComponent<NavMeshAgent>().enabled = false;
            _isDead = true;
            this.GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject);
            
            
        }
        else
        {
            _bloodGO=  Instantiate(Resources.Load<GameObject>("FX_ROOT"), this.transform.position, this.transform.rotation);
            _bloodGO.transform.SetParent(this.transform);
            _bloodGO.SetActive(true);
            StartCoroutine(BloodFX());
            SoundManager.Instance.PlaySound(SoundManager.Instance.enemySound);
        }
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
  
    private IEnumerator BloodFX()
    {
        yield return new WaitForSeconds(1f);
        _bloodGO.SetActive(false);
        Destroy(_bloodGO);
    }

}