using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float runSpeed = 18f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck; 
    public float groundDistance = 0.4f; 
    public LayerMask groundMask;
    
    Vector3 velocity;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    private bool isMoving;
    private bool isRunning;
    bool isGrounded;

    private AudioSource audioSource;

    public float maxSlopeAngle = 45f; 

    private float lastGroundedY; 
    public float fallDamageThreshold = 5f; 
    public float damageMultiplier = 2f; 

    private bool isOnIce = false; 
    private float iceSlideSpeed = 0f; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (DialogSystem.Instance.dialogUIActive == false)
        {
            Movement();
        }
        
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            JumpToNextLevel();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            JumpToPreviousLevel();;
        }
    }

    public void SetIsOnIce(bool onIce, float slideSpeed)
    {
        isOnIce = onIce;
        iceSlideSpeed = slideSpeed;
    }

    public void Movement()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!wasGrounded && isGrounded)
        {
            float fallDistance = lastGroundedY - transform.position.y;
            if (fallDistance > fallDamageThreshold)
            {
                ApplyFallDamage(fallDistance);
            }
        }

        if (isGrounded)
        {
            lastGroundedY = transform.position.y; 
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isOnIce)
        {
            
            Vector3 slideDirection = new Vector3(move.x, 0, move.z).normalized;
            controller.Move(slideDirection * iceSlideSpeed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            if (IsOnSlope(out RaycastHit slopeHit))
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                Debug.Log("Slope angle: " + slopeAngle);

                if (slopeAngle > maxSlopeAngle)
                {
                    Vector3 slideDirection = Vector3.ProjectOnPlane(move, slopeHit.normal).normalized;
                    controller.Move(slideDirection * speed * Time.deltaTime);
                    Debug.Log("Sliding down slope.");
                }
                else
                {
                    controller.Move(move * speed * Time.deltaTime);
                    Debug.Log("Moving normally on slope.");
                }
            }
            else
            {
                controller.Move(move * speed * Time.deltaTime);
                Debug.Log("Moving normally.");
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                SoundManager.Instance.PlaySound(SoundManager.Instance.jumpingSound);
            }

            if (Input.GetKey(KeyCode.LeftShift) &&isGrounded && PlayerState.Instance.currentCalories > 0)
            {
                isRunning = true;
                SoundManager.Instance.PlaySound(SoundManager.Instance.runningSound);
                speed = runSpeed;
            }
            else
            {
                isRunning = false;
                SoundManager.Instance.runningSound.Stop();
                speed = 12f;
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }

        if (lastPosition != gameObject.transform.position && isGrounded && !isRunning)
        {
            isMoving = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.walkingSound);
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.walkingSound.Stop();
        }

        lastPosition = gameObject.transform.position;
    }

    private bool IsOnSlope(out RaycastHit slopeHit)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, controller.height / 2 * 1.1f, groundMask))
        {
            return Vector3.Angle(slopeHit.normal, Vector3.up) > 0f;
        }
        return false;
    }

    private void ApplyFallDamage(float fallDistance)
    {
        float damage = (fallDistance - fallDamageThreshold) * damageMultiplier;
        PlayerState.Instance.currentHealth -= (int)damage; 
    }

    private void JumpToNextLevel()
    {
        SaveManager.Instance.SaveGame(0);
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(++sceneIndex);
    }
    
    private void JumpToPreviousLevel()
    {
        SaveManager.Instance.SaveGame(0);
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(--sceneIndex);
    }
    
}
