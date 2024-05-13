
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;



public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance { get; private set; }

    public TextMeshProUGUI dialogText;
    

    public Canvas dialogUI;

    public bool dialogUIActive;
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
        DontDestroyOnLoad(gameObject);
        
    }


    public void OpenDialogUI()
    {
       
        dialogUIActive = true;
        if (dialogUIActive == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
        }
           
        
        
        dialogUI.gameObject.SetActive(true);
    }
    
    public void CloseDialogUI()
    {
       
        dialogUIActive = false;

        dialogUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
