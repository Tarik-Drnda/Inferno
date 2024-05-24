using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleThreeScript : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject infoTab;
    public GameObject infoTabText;

    private bool _isDead;
    public GameObject pressF;

    private GameObject gm;
    void Start()
    {
        SaveManager.Instance.LoadGame(0);
        gm = GameObject.FindWithTag("NPC2");
        StartCoroutine(DisplayInfo());
    }

    void Update()
    {
        infoTabText.GetComponent<Text>().text =
            "Kill the enemies in order to enter the third circle. \n Enemies remaining: " + enemies.Count;
        for(int i=0;i<enemies.Count;i++)
        {
            if (enemies[i]==null)
            {
                enemies.Remove(enemies[i]);
            }
        }
        if (enemies.Count == 0)
        {
            infoTabText.GetComponent<Text>().text = "Talk with NPC to continue";
            gm.SetActive(true);
            AllGameData data = new AllGameData();
            if (gm.GetComponent<SecondNPC>().complitedDialog == true)
            {
                data.playerData = GetUpdatedPlayerDataForNextLevel();
                data.enviromentData = SaveManager.Instance.getEnviromentData();
                SaveManager.Instance.SavingTypeSwitch(data, 0);
                SceneManager.LoadScene("4.krug2");
            }
        }
        else
        {
            
            gm.SetActive(false);
        }
        
        
        
    }
    public IEnumerator DisplayInfo()
    {
        yield return new WaitForSeconds(5f);
        infoTab.SetActive(true);
    }
    
    private PlayerData GetUpdatedPlayerDataForNextLevel()
    {
        float[] playerStats = new float[2];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;

        float[] newPlayerPosAndRot = new float[6];
        newPlayerPosAndRot[0] = 500.6f; 
        newPlayerPosAndRot[1] = 35f;  
        newPlayerPosAndRot[2] = 45.32f; 

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
