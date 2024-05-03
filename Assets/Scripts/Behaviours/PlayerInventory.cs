using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> inventoryList = new List<GameObject>();

    public static PlayerInventory Instance { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
        }
        
    }
}
