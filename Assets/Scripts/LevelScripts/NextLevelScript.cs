using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelScript : MonoBehaviour
{
    public bool playerInRange;

    public GameObject _keyF;
    public GameObject _tabInfo;
    public GameObject frame;
    
    public GameObject _npc1;
    public GameObject _npc2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (playerInRange == true)
        {
            _keyF.SetActive(true);
        }
        else
        {
            _keyF.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F) && playerInRange == true 
                                        && NPC.Instance.ComplitedDialog==true
                                        && SecondNPC.Instance.complitedDialog==true)
        {
            
           
            SceneManager.LoadScene("1.krug");
        }
        else if ( playerInRange == true
                                             && GameObject.FindWithTag("NPC1").GetComponent<NPC>().ComplitedDialog==false
                                             && GameObject.FindWithTag("NPC2").GetComponent<SecondNPC>().complitedDialog==true)
        {
            _tabInfo.GetComponent<Text>().text = "Complete dialog with NPC1!";
            _tabInfo.SetActive(true);
            frame.SetActive(true);
         
            
        }
        else if (playerInRange == true
                                             && GameObject.FindWithTag("NPC1").GetComponent<NPC>().ComplitedDialog==true
                                             && GameObject.FindWithTag("NPC2").GetComponent<SecondNPC>().complitedDialog==false)
        {
            _tabInfo.GetComponent<Text>().text = "Complete dialog with NPC2!";
            _tabInfo.SetActive(true);
            frame.SetActive(true);
          
            
        }
        else if (playerInRange == true
                                             && GameObject.FindWithTag("NPC1").GetComponent<NPC>().ComplitedDialog==false
                                             && GameObject.FindWithTag("NPC2").GetComponent<SecondNPC>().complitedDialog==false)
        {
            
            _tabInfo.GetComponent<Text>().text="Complete dialog with NPC1!" + "\n" + "Complete dialog with NPC2!";
            _tabInfo.SetActive(true);
            frame.SetActive(true);
            
           
        }
        else
        {
            _tabInfo.SetActive(false);
            frame.SetActive(false);
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
    
}
