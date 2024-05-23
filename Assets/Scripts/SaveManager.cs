using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }

    private string fileName = "SaveGame";
    public bool isLoading;
    public Canvas loadingScreen;

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
    
    
     string jsonPathProject;
     string jsonPathPersistant;
     string binaryPath;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar ;
    }

    public bool isSavnigJason;
    #region || ------General Section------||
    #region || ------Saving------||
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = getPlayerData();
        data.enviromentData = getEnviromentData();
        data.sceneName = SceneManager.GetActiveScene().name; 
        SavingTypeSwitch(data,slotNumber);
    }

    private EnviromentData getEnviromentData()
    {
        List<string> itemsPickedup = InventorySystem.Instance.itemsPickedup;
        return new EnviromentData(itemsPickedup);
    }

    private PlayerData getPlayerData()
    {
        float[] playerStats = new float[2];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        
        

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;
        
        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        string[] inventory = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlots = GetQuickSlotsContent();
        return new PlayerData(playerStats, playerPosAndRot,inventory,quickSlots);
    }

    private string[] GetQuickSlotsContent()
    {
        List<string> temp = new List<string>();
        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if (slot.transform.childCount != 0)
            {
                string name  = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName=name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }
        return temp.ToArray();
    }

    public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
    {
        if (isSavnigJason)
        {
           SaveGameDataToJsonFile(gameData,slotNumber);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData,slotNumber);
        }
    }
    #endregion
    #region || ------Loading------||

    public AllGameData LoadingTypeSwitch(int slotNumber)
    {
        if (isSavnigJason)
        {
            AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber)
    {
        SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);
        SetEnviromentData(LoadingTypeSwitch(slotNumber).enviromentData);
        
        isLoading = false;
        
        DisableLoadingScreen();


    }

    public void SetEnviromentData(EnviromentData enviromentData)
    {
        foreach (Transform itemType in EnviromentManager.Instance.allItems.transform)
        {
            foreach (Transform item in itemType.transform)
            {
                if (enviromentData.pickedupItems.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        InventorySystem.Instance.itemsPickedup = enviromentData.pickedupItems;
    }

    public void SetPlayerData(PlayerData playerData)
    {

        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories =playerData.playerStats[1];
      
        Vector3 loadedPosition;
        loadedPosition.x =playerData.playerPositionAndRotation[0];
        loadedPosition.y =playerData.playerPositionAndRotation[1];
        loadedPosition.z =playerData.playerPositionAndRotation[2];
        PlayerState.Instance.playerBody.transform.position =  loadedPosition;
        Vector3 loadedRotation;
        loadedRotation.x =  playerData.playerPositionAndRotation[3]; 
        loadedRotation.y =  playerData.playerPositionAndRotation[4]; 
        loadedRotation.z =  playerData.playerPositionAndRotation[5];
        PlayerState.Instance.playerBody.transform.rotation =  Quaternion.Euler(loadedRotation);
        
        foreach (string item in playerData.inventoryContent)
        {
            InventorySystem.Instance.AddToInventory(item);
        }
        foreach (string item in playerData.quickSlotsContent)
        {
            GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));
            
            itemToAdd.transform.SetParent(availableSlot.transform,false);
        }
    }


    public void StartLoadedGame(int slotNumber)
    {
        ActivateLoadingScreen();
        isLoading = true;
        AllGameData data = LoadingTypeSwitch(slotNumber);
        string sceneToLoad = data.sceneName;
        SceneManager.LoadScene(sceneToLoad);
        StartCoroutine(DelayedLoading(slotNumber));
    }

    private IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1f);
        LoadGame(slotNumber);
        print("Game Loaded");
    }

    #endregion
    #endregion

    #region || ------Binary Section------||

    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();
       
        FileStream stream = new FileStream(binaryPath  + fileName + slotNumber + ".bin", FileMode.Create);
        
        formatter.Serialize(stream,gameData);
        stream.Close();
        print("Data saved to" + binaryPath  + fileName + slotNumber + ".bin" );
    }
    public AllGameData LoadGameDataFromBinaryFile( int slotNumber)
    {
        if (File.Exists(binaryPath+ fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            FileStream stream = new FileStream(binaryPath+ fileName + slotNumber + ".bin", FileMode.Open);
            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();
            print("Data loaded from: " + binaryPath + fileName + slotNumber + ".bin" );
            return data;
        }
        else
        {
            return null;
        }
    }
    #endregion
    
    #region || ------Json Section------||

    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);

        string encrypted = EncryptionDecryption(json);
        using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            writer.Write(json);
            print("saved game to Json file at: " + jsonPathProject+ fileName + slotNumber + ".json");
        }
    }
    public AllGameData LoadGameDataFromJsonFile( int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();
            string decrypted = EncryptionDecryption(json);
            AllGameData data = JsonUtility.FromJson<AllGameData>(json);
            return data;
        }
    }
    #endregion
    
    #region || ------Settings Section------||
    #region || ------Volume Settings------||
    [System.Serializable]
    public class  VolumeSettings
    {
        public float music;
        public float effects;
        public float master;
    }

    public void SaveVolumeSetings(float _music, float _effects, float _master)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            music = _music,
            effects = _effects,
            master = _master
        };
        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();
        print("Saved to Player Pref");
    }

    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
        
    }
    #endregion

    #endregion
    
    #region || ------Encryption------||

    public string EncryptionDecryption(string jsonString)
    {
        string keyword = "1234567";

        string result = "";

        for (int i = 0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }

        return result;
    }
    #endregion
    
    #region || ------Utility------||

    public bool DoesFileExist(int slotNumber)
    {
        if (isSavnigJason)
        {
            if (File.Exists(jsonPathProject+ fileName + slotNumber +".json"))
            {
                return true;
            }
            else
            {
                return false;

            }
        }
        else
        {
            if (File.Exists(binaryPath + fileName + slotNumber +".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsSlotEmpty(int slotNumber)
    {
        if (DoesFileExist(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
    #endregion
    
    #region || ------Loading Section------||

    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    public void DisableLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    #endregion

    
}
