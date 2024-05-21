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
    // Start is called before the first frame update
    void Start()
    {
        infoTab.SetActive(true);
        infoTabText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        infoTabText.GetComponent<Text>().text = "Find a way to exit a graveyard";
        if (playerInRange == true)
        {
            pressF.SetActive(true);
            if(Input.GetKeyDown((KeyCode.F)))
                SceneManager.LoadScene("7.krug");
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
