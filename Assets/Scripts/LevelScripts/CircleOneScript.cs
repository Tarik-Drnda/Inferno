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
                SaveManager.Instance.SaveGame(0);
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
}
