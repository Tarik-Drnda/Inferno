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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplayInfo());
    }

    // Update is called once per frame
    void Update()
    {
        infoTabText.GetComponent<Text>().text = "Find a way to exit a gold canyon";
        if (playerInRange == true)
        {
            pressF.SetActive(true);
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
}
