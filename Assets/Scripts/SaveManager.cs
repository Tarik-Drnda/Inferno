using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }

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
    
    
//Json project save path
     string jsonPathProject;
//Json external/real save path
     string jsonPathPersistant;
//binary save path
     string binaryPath;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        binaryPath = Application.persistentDataPath + "/save_game.bin";
    }

    public bool isSavnigJason;
    #region || ------General Section------||
    #region || ------Saving------||
    public void SaveGame()
    {
        AllGameData data = new AllGameData();

        data.playerData = getPlayerData();
        SavingTypeSwitch(data);
    }

    private PlayerData getPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        
        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;
        
        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        return new PlayerData(playerStats, playerPosAndRot);
    }

    public void SavingTypeSwitch(AllGameData gameData)
    {
        if (isSavnigJason)
        {
           SaveGameDataToJsonFile(gameData);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }
    #endregion
    #region || ------Loading------||

    public AllGameData LoadingTypeSwitch()
    {
        if (isSavnigJason)
        {
            AllGameData gameData = LoadGameDataFromJsonFile();
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
    }

    public void LoadGame()
    {
        //player data
        SetPlayerData(LoadingTypeSwitch().playerData);
    }

    private void SetPlayerData(PlayerData playerData)
    {

// Setting Player Stats
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories =playerData.playerStats[1];
      
// Setting Player Position
        Vector3 loadedPosition;
        loadedPosition.x =playerData.playerPositionAndRotation[0];
        loadedPosition.y =playerData.playerPositionAndRotation[1];
        loadedPosition.z =playerData.playerPositionAndRotation[2];
        PlayerState.Instance.playerBody.transform.position =  loadedPosition;
// Setting Player Rotation
        Vector3 loadedRotation;
        loadedRotation.x =  playerData.playerPositionAndRotation[3]; 
        loadedRotation.y =  playerData.playerPositionAndRotation[4]; 
        loadedRotation.z =  playerData.playerPositionAndRotation[5];
        PlayerState.Instance.playerBody.transform.rotation =  Quaternion.Euler(loadedRotation);
    }


    public void StartLoadedGame()
    {
        SceneManager.LoadScene("0.krug");
        StartCoroutine(DelayedLoading());
    }

    private IEnumerator DelayedLoading()
    {
        yield return new WaitForSeconds(1f);
        LoadGame();
        print("Game Loaded");
    }

    #endregion
    #endregion

    #region || ------Binary Section------||

    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //find path and on that path create filemode
       
        FileStream stream = new FileStream(binaryPath, FileMode.Create);
        
        formatter.Serialize(stream,gameData);
        stream.Close();
        //gdje je spremljeno sve sto imamo od podataka 
        print("Data saved to" + Application.persistentDataPath + "/save_game.bin" );
    }
    public AllGameData LoadGameDataFromBinaryFile()
    {
        //loadamo sa istog mjesta
        if (File.Exists(binaryPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            FileStream stream = new FileStream(binaryPath, FileMode.Open);
            //uzima sve podatke i bilda allgamedata
            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();
            print("Data loaded from" + binaryPath );
            return data;
        }
        else
        {
            return null;
        }
    }
    #endregion
    
    #region || ------Json Section------||

    public void SaveGameDataToJsonFile(AllGameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);

        string encrypted = EncryptionDecryption(json);
        using (StreamWriter writer = new StreamWriter(jsonPathProject))
        {
            writer.Write(encrypted);
            print("saved game to Json file at: " + jsonPathProject);
        }
    }
    public AllGameData LoadGameDataFromJsonFile()
    {
        using (StreamReader reader = new StreamReader(jsonPathProject))
        {
            string json = reader.ReadToEnd();
            string decrypted = EncryptionDecryption(json);
            AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted);
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
        //as json string, because that is the only way 
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
        //json file da se ne moze citati
        string keyword = "1234567";

        string result = "";

        for (int i = 0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }

        return result;
    }
    #endregion
}
