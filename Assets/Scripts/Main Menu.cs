using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void NewGame()
    {
        SceneManager.LoadScene("0.krug");
    }

    // Update is called once per frame
    public void ExitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
