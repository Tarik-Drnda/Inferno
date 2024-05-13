using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    private int currentDialogueIndex = 0;
    private string[] dialogues;
    private AudioSource[] dialogueAudioSources;
    Animator animator;

    public AudioSource dialog1Audio;
    public AudioSource dialog2Audio;
    public AudioSource dialog3Audio;
    public AudioSource dialog4Audio;
    public AudioSource dialog5Audio;

    private void Start()
    {
        animator = GetComponent<Animator>();
        dialogues = new string[]
        {
            "Welcome to the depths of Hell, traveler. I am Minos, the guardian of these infernal realms and the arbiter of the circles of Hell. It appears that you have wandered into this dark domain by chance, have you not?",
            "But fret not, for every soul has its own path to traverse. Despite the accidental nature of your arrival, I shall grant you passage to continue onward. You are permitted to face the trials that lie ahead, to strive for your eventual return to the realm of the living.",
            "Be warned, however, that the path ahead is fraught with peril. Many obstacles await you, and the challenges you will encounter are not to be taken lightly. You must steel yourself for the trials ahead, for they will test your courage and resolve to their limits.",
            "Yet, take heart, brave traveler. Though the road may be treacherous, it is not impassable. With determination and perseverance, you may yet find your way through this labyrinth of suffering. Face each challenge with resolve, for it is through adversity that one's true strength is revealed.",
            "Go forth now, into the shadows that shroud this realm. May you emerge from the fires of Hell unscathed, your spirit unbroken by the trials that await. Remember, it is the courageous who emerge triumphant from the depths of despair."
        };

        dialogueAudioSources = new AudioSource[]
        {
            dialog1Audio,
            dialog2Audio,
            dialog3Audio,
            dialog4Audio,
            dialog5Audio
        };
    }

    private void Update()
    {
        if (isTalkingWithPlayer)
        {
            animator.SetBool("isTalking",true);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                NextDialogue();
            }
        }

    }

    public void StartConversation()
    {
        isTalkingWithPlayer = true;
        LookAtPlayer();
        Debug.Log("Conversation started");

        DialogSystem.Instance.OpenDialogUI();
        ShowCurrentDialogue();
    }

    void NextDialogue()
    {
        StopCurrentDialogueAudio();

        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
        {
            ShowCurrentDialogue();
        }
        else
        {
            CloseConversation();
        }
    }


public void LookAtPlayer()
    {
        var player = PlayerState.Instance.playerBody.transform;
        Vector3 direction = player.position - transform.position; 
        transform.rotation = Quaternion.LookRotation(direction);
        
        var yRotation  = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler (0,yRotation, 0);
    }
    void ShowCurrentDialogue()
    {
        DialogSystem.Instance.dialogText.text = dialogues[currentDialogueIndex] + "\nPress Q to go next";

        if (currentDialogueIndex < dialogueAudioSources.Length)
        {
            dialogueAudioSources[currentDialogueIndex].Play();
        }
    }

    void CloseConversation()
    {
        DialogSystem.Instance.CloseDialogUI();
        isTalkingWithPlayer = false;
        currentDialogueIndex = 0;
        animator.SetBool("isTalking",false);

    }

    void StopCurrentDialogueAudio()
    {
        if (currentDialogueIndex >= 0 && currentDialogueIndex < dialogueAudioSources.Length)
        {
            dialogueAudioSources[currentDialogueIndex].Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            currentDialogueIndex = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
