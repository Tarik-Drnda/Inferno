using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleEightScript : MonoBehaviour
{
    public GameObject infoTab;
    public GameObject infoTabText;
    public GameObject gm;
    private bool playerInRange;
    void Start()
    {
        SaveManager.Instance.LoadGame(0);
        infoTab.SetActive(true);
        infoTabText.SetActive(true);
    }

    void Update()
    {
        infoTabText.GetComponent<Text>().text = "Defeat Demons to exit the arena!";
        if (gm == null)
        {
            StartCoroutine(StartAnotherLevel());
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

    private IEnumerator StartAnotherLevel()
    {
        yield return new WaitForSeconds(3f);
        AllGameData data = new AllGameData();
        data.playerData = GetUpdatedPlayerDataForNextLevel();
        data.enviromentData = SaveManager.Instance.getEnviromentData();
        SaveManager.Instance.SavingTypeSwitch(data,0);
        SceneManager.LoadScene("9.krug2");
    }
    
    private PlayerData GetUpdatedPlayerDataForNextLevel()
    {
        float[] playerStats = new float[2];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;

        float[] newPlayerPosAndRot = new float[6];
        newPlayerPosAndRot[0] = 336.8f; 
        newPlayerPosAndRot[1] = 37.9f;  
        newPlayerPosAndRot[2] = 246.9f; 

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
