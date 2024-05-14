using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecondNPC : MonoBehaviour

{
     public bool playerInRange;
    public bool isTalkingWithPlayer2;

    private int currentDialogueIndex = 0;
    private string[] dialogues;
    private AudioSource[] dialogueAudioSources;
    Animator animator;

     public AudioSource dialog1Audio;
    public AudioSource dialog2Audio;
    public AudioSource dialog3Audio;
    public AudioSource dialog4Audio;

    private void Start()
    {
        animator = GetComponent<Animator>();
        dialogues = new string[]
        {
            "Ah, traveler, you find yourself halfway through the depths of Hell, the realm of eternal suffering and despair. But fear not, for I am here to offer you aid on your perilous journey.",
            "As you traverse these infernal landscapes, know that the path ahead is fraught with danger, and the challenges you face are immense. However, with my guidance and the tools I provide, you can find the strength to overcome even the darkest of trials.",
            "Beside me lie potions and weapons, imbued with the power to assist you in your task. Drink the elixirs to regain strength, and wield the weapons to defend yourself against the demons that haunt these accursed halls.",
            "But remember, the road to salvation is long and arduous. While my aid may lighten your burden, ultimately, courage and determination are what will carry you through. Brave forth, brave soul, and press on. Victory awaits those who dare to confront the depths of Hell and emerge triumphant on the other side."
          
        };

       dialogueAudioSources = new AudioSource[]
        {
            dialog1Audio,
            dialog2Audio,
            dialog3Audio,
            dialog4Audio,
        };
    }

 

    private void Update()
    {
        if (isTalkingWithPlayer2)
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
        isTalkingWithPlayer2 = true;
        Debug.Log("Conversation started");
        LookAtPlayer();

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

    private void StopCurrentDialogueAudio()
    {
        if (currentDialogueIndex >= 0 && currentDialogueIndex < dialogueAudioSources.Length)
        {
            dialogueAudioSources[currentDialogueIndex].Stop();
        }
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
        isTalkingWithPlayer2 = false;
        currentDialogueIndex = 0;
        animator.SetBool("isTalking", false);
    }

    public void LookAtPlayer()
    {
        var player = PlayerState.Instance.playerBody.transform;
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
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
