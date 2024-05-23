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
        SaveManager.Instance.SaveGame(0);
        SceneManager.LoadScene("9.krug2");
    }
}
