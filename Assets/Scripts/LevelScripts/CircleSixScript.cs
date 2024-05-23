using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleSixScript : MonoBehaviour
{
    public GameObject infoTab;
    public GameObject infoTabText;
    public LayerMask waterLayer;
    private bool playerInRange;
    public GameObject pressF;
    void Start()
    {
        SaveManager.Instance.LoadGame(0);
        infoTab.SetActive(true);
        infoTabText.SetActive(true);
    }

    void Update()
    {
        infoTabText.GetComponent<Text>().text = "Find a way to exit a graveyard";
        if (playerInRange == true)
        {
            pressF.SetActive(true);

            if (Input.GetKeyDown((KeyCode.F)))
            {
                SaveManager.Instance.SaveGame(0);
                SceneManager.LoadScene("7.krug2");
            }
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
}
