using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NextLevelScript : MonoBehaviour
{
    public bool playerInRange;

    [FormerlySerializedAs("_keyF")] public GameObject keyF;
    [FormerlySerializedAs("_tabInfo")] public GameObject tabInfo;
    public GameObject frame;

    public GameObject wp;
    
    void Start()
    {
        StartCoroutine(DisplayInfo());
    }

    void Update()
    {
       if (wp.gameObject == null)
       {
           tabInfo.GetComponent<Text>().text = "Enter through portal in tomb";
       }
       else
       {
           tabInfo.SetActive(true);
           tabInfo.GetComponent<Text>().text = "Search a map and find a weapon. \n Enter through portal in tomb";
       }
        if (playerInRange == true)
        {
            keyF.SetActive(true);
        }
        else
        {
            keyF.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.F) && playerInRange == true )
        {
            SceneManager.LoadScene("1.krug");
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

    public IEnumerator DisplayInfo()
    {
        yield return new WaitForSeconds(3.5f);
        frame.SetActive(true);
    }
    
}
