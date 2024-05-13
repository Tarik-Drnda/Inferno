using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float runSpeed = 18f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck; //Provjerava da li je na zemlji
    public float groundDistance = 0.4f; //Provjerava udaljenost igraca od zemlje
    public LayerMask groundMask;
    
    Vector3 velocity;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    private bool isMoving;
    private bool isRunning;
    bool isGrounded;
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       if (DialogSystem.Instance.dialogUIActive==false)
        {
            Movement();
        }
        
    }
public void Movement()
    {
        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //right is the red Axis, foward is the blue axis
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //check if the player is on the ground so he can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            SoundManager.Instance.PlaySound(SoundManager.Instance.jumpingSound);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && PlayerState.Instance.currentCalories > 0)
        {
            isRunning = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.runningSound);
            PlayerState.Instance.playerBody.GetComponent<PlayerMovement>().speed = 18f;
        }
        else
        {
            isRunning = false;
            PlayerState.Instance.playerBody.GetComponent<PlayerMovement>().speed = 12f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded ==true && isRunning==false)
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

}
