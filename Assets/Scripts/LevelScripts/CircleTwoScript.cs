using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleTwoScript : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject infoTab;
    public GameObject infoTabText;

    private bool playerInRange;
    public GameObject pressF;
    void Start()
    {
        SaveManager.Instance.LoadGame(0);
    }

    void Update()
    {
        if (playerInRange == true)
        {
            pressF.SetActive(true);
            SaveManager.Instance.SaveGame(0);
            SceneManager.LoadScene("3.krug2");
        }
        else
        {
            pressF.SetActive(false);
        }

        if (NPC.Instance.ComplitedDialog)
        {
            infoTabText.GetComponent<Text>().text = "Find a way to exit a ice canyon";
        }
        else
        {
            infoTabText.GetComponent<Text>().text = "Find a way to exit a ice canyon \n Talk with NPC";
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
