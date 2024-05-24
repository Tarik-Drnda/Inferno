using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleOneScript : MonoBehaviour
{
    public GameObject infoTab;
    public GameObject infoTabText;
    public GameObject pressF;

    private bool _isDead;
    public List<GameObject> enemies = new List<GameObject>();

    private bool playerInRange;
    void Start()
    {
       SaveManager.Instance.LoadGame(0);
        StartCoroutine(DisplayInfo());
    }

    void Update()
    {
        infoTabText.GetComponent<Text>().text =
            "Kill the enemies in order to enter the castle Portal. \n Enemies remaining: " + enemies.Count;
       for(int i=0;i<enemies.Count;i++)
        {
            Debug.Log(_isDead);
            if (enemies[i].gameObject==null)
            {
                enemies.Remove(enemies[i]);
            }
        }

        if (enemies.Count == 0)
        {
            infoTabText.GetComponent<Text>().text = "Enter the Castle protal to get into a another circle";
        }
        if (enemies.Count == 0 && playerInRange==true)
        {
            pressF.SetActive(true);
            SelectionManager.Instance.Crosshair.SetActive(false);
            if (Input.GetKeyDown(KeyCode.F) && enemies.Count == 0)
            {
                AllGameData data = new AllGameData();
                data.playerData = GetUpdatedPlayerDataForNextLevel();
                data.enviromentData = SaveManager.Instance.getEnviromentData();
                SaveManager.Instance.SavingTypeSwitch(data,0);
                
                SceneManager.LoadScene("2.krug2");
            }
        }
        else
        {
            pressF.SetActive(false);
            SelectionManager.Instance.Crosshair.SetActive(true);
        }
    }

    public IEnumerator DisplayInfo()
    {
        yield return new WaitForSeconds(5f);
        infoTab.SetActive(true);
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
    
    private PlayerData GetUpdatedPlayerDataForNextLevel()
    {
        float[] playerStats = new float[2];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;

        float[] newPlayerPosAndRot = new float[6];
        newPlayerPosAndRot[0] = 481.57f; 
        newPlayerPosAndRot[1] = 21.57f;  
        newPlayerPosAndRot[2] = 27.92f; 

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
