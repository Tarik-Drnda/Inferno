using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button LoadGameBTN;

    private void Start()
    {
        LoadGameBTN.onClick.AddListener(() =>
        {
            SaveManager.Instance.StartLoadedGame();
        });
    }

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
