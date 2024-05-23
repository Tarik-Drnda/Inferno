using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleSevenScript : MonoBehaviour
{
    public GameObject infoTab;
    public GameObject infoTabText;
    private GameObject gm;
    private bool playerInRange;
    void Start()
    {
        SaveManager.Instance.LoadGame(0);
        infoTab.SetActive(true);
        infoTabText.SetActive(true);
        gm = GameObject.FindWithTag("Water");
    }

    void Update()
    {
        if (playerInRange == true)
        {
            SaveManager.Instance.SaveGame(0);
           SceneManager.LoadScene("8.krug2");
        }
        infoTabText.GetComponent<Text>().text = "Stay in the boat!";
        if (gm.GetComponent<BloodWater>().playerInRange == true)
        {
            PlayerState.Instance.currentHealth = 0;
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
