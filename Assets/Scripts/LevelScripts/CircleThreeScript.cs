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
            SaveManager.Instance.SaveGame(0);
            SceneManager.LoadScene("4.krug2");
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
}
