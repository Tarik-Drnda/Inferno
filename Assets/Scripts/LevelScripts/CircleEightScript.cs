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
    // Start is called before the first frame update
    void Start()
    {
        infoTab.SetActive(true);
        infoTabText.SetActive(true);
        
    }

    // Update is called once per frame
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
        SceneManager.LoadScene("9.krug");
    }
}
