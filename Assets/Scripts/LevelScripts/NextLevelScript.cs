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
           tabInfo.GetComponent<Text>().text = "Enter through portal in tomb \n You will be aided through the game by your ally Vergilius";
       }
       else
       {
           tabInfo.SetActive(true);
           tabInfo.GetComponent<Text>().text = "Search a map and find a weapon. \n Enter through portal in tomb \n You will be aided through the game by your ally Vergilius";
       }
        if (playerInRange == true)
        {
            keyF.SetActive(true);
            SelectionManager.Instance.Crosshair.SetActive(false);
        }
        else
        {
            keyF.SetActive(false);
            SelectionManager.Instance.Crosshair.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F) && playerInRange == true && wp.gameObject==null)
        {
            AllGameData data = new AllGameData();

            data.playerData = GetUpdatedPlayerDataForNextLevel();
            data.enviromentData = SaveManager.Instance.getEnviromentData();
               
            SaveManager.Instance.SavingTypeSwitch(data,0);
            
            SceneManager.LoadScene("1.krug2");
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
    private PlayerData GetUpdatedPlayerDataForNextLevel()
    {
        float[] playerStats = new float[2];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;

        float[] newPlayerPosAndRot = new float[6];
        newPlayerPosAndRot[0] = 32.7f; 
        newPlayerPosAndRot[1] = 2f;  
        newPlayerPosAndRot[2] = 25.2f; 

        newPlayerPosAndRot[3] = 0f;  
        newPlayerPosAndRot[4] = 0f;  
        newPlayerPosAndRot[5] = 0f;  

        string[] inventory = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlots = GetQuickSlotsContent();

        return new PlayerData(playerStats, newPlayerPosAndRot, inventory, quickSlots);
    }

    private string[] GetQuickSlotsContent()
    {
        List<string> temp = new List<string>();
        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if (slot.transform.childCount != 0)
            {
                string name  = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName=name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }
        return temp.ToArray();
    }

}
