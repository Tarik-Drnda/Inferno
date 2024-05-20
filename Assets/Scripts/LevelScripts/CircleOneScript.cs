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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplayInfo());
    }

    // Update is called once per frame
    void Update()
    {
        infoTabText.GetComponent<Text>().text =
            "Kill the enemies in order to enter the castle Portal. \n Enemies remaining: " + enemies.Count;
       for(int i=0;i<enemies.Count;i++)
        {
            
            _isDead = enemies[i].gameObject.GetComponent<EnemyAI>()._isDead;
            Debug.Log(_isDead);
            if (_isDead==true)
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
            if (Input.GetKeyDown(KeyCode.F) && enemies.Count == 0)
            {
                
                SceneManager.LoadScene("2.krug");
            }
        }
        else
        {
            pressF.SetActive(false);
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