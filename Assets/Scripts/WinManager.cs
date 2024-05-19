using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance { get; private set; }

    public GameObject winCanvas;
    private bool isGameWin = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ShowWinCanvas()
    {
        if (!isGameWin)
        {
            winCanvas.SetActive(true);
            SoundManager.Instance.startingZoneBGMusic.Stop();
            SoundManager.Instance.PlaySound(SoundManager.Instance.menuSound);
            isGameWin = true;
            Invoke("ReturnToMainMenu", 7f); 
           
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}