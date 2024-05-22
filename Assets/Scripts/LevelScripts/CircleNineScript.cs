using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleNineScript : MonoBehaviour
{
    public GameObject infoTab;
    public GameObject infoTabText;
    public List<GameObject> enemies = new List<GameObject>();
    private bool _isDead;
    private bool _audioPlayed=false;
    public AudioSource endDialog;

    public AudioSource dialog1;
    private bool _dialogPlayed1=false;
    public AudioSource dialog2;
    private bool _dialogPlayed2=false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplayInfo());
    }

    // Update is called once per frame
    void Update()
    {
        if (_dialogPlayed1 == false)
        {
            dialog1.Play();
            _dialogPlayed1 = true;
            StartCoroutine(PlayDialog2());
        }

 
        infoTabText.GetComponent<Text>().text =
            "Kill the enemies in order to exit the Inferno. \n Enemies remaining: " + enemies.Count;
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
            infoTabText.GetComponent<Text>().text = "You Won! You defeated your self and menage to exit the Inferno";
            if (_audioPlayed == false)
            {
                endDialog.Play();
                _audioPlayed = true;
            }
            
            StartCoroutine(DisplayWinCanvas());
        }
    }

    private IEnumerator DisplayInfo()
    {
        yield return new WaitForSeconds(33f);
        infoTab.SetActive(true);
        infoTabText.SetActive(true);
    }

    private IEnumerator DisplayWinCanvas()
    {
        yield return new WaitForSeconds(7f);
        WinManager.Instance.ShowWinCanvas();
    }

    private IEnumerator PlayDialog2()
    {
        yield return new WaitForSeconds(24f);
        dialog1.Stop();
        dialog2.Play();
    }
}
