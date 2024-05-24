using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleFourScript : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject infoTab;
    public GameObject infoTabText;

    private bool playerInRange;
    public GameObject pressF;
    void Start()
    {
        SaveManager.Instance.LoadGame(0);
        StartCoroutine(DisplayInfo());
    }

    void Update()
    {
        infoTabText.GetComponent<Text>().text = "Find a way to exit a gold canyon";
        if (playerInRange == true)
        {
            pressF.SetActive(true);
            AllGameData data = new AllGameData();
            data.playerData = GetUpdatedPlayerDataForNextLevel();
            data.enviromentData = SaveManager.Instance.getEnviromentData();
            SaveManager.Instance.SavingTypeSwitch(data,0);
            SceneManager.LoadScene("5.krug2");
        }
        else
        {
            pressF.SetActive(false);
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

    private IEnumerator DisplayInfo()
    {
        yield return new WaitForSeconds(7.5f);
        infoTab.SetActive(true);
        infoTabText.SetActive(true);
    }
    
    private PlayerData GetUpdatedPlayerDataForNextLevel()
    {
        float[] playerStats = new float[2];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;

        float[] newPlayerPosAndRot = new float[6];
        newPlayerPosAndRot[0] = 2.328638f; 
        newPlayerPosAndRot[1] = 6.370003f;  
        newPlayerPosAndRot[2] = -17.38016f; 

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
