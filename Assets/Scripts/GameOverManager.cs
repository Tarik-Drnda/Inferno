using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public GameObject deathCanvas;
    private bool isGameOver = false;

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

    public void ShowDeathCanvas()
    {
        if (!isGameOver)
        {
            deathCanvas.SetActive(true);
            //SoundManager.Instance.startingZoneBGMusic.Stop();
            
            SoundManager.Instance.StopAllSoundsAndPlayDesired(SoundManager.Instance.menuSound);
           // SoundManager.Instance.PlaySound(SoundManager.Instance.menuSound);
            isGameOver = true;
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