using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public GameObject ItemInfoUI;
    
    public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    public bool isOpen;
  
  
    //PICKUP POPUP

    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public List<string> itemsPickedup;
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
 
 
    void Start()
    {
       isOpen = false;
     
        PopulateSlotList();

        Cursor.visible = false;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }


    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.I) && isOpen==false)
        {
 
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
           SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;
            Debug.Log(isOpen);
 
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen==true)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //ovdje je greska kod selectiona 
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            isOpen = false;
        }
    }


    public void AddToInventory(string itemName)
    {
        if (SaveManager.Instance.isLoading == false)
        {
            //muzika SoundManager.Instance.PlaySound(SoundManager.Instacne.pickItemSound);
        }
        
            whatSlotToEquip = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName),
                whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            TriggerPickUpPop(itemName,itemToAdd.GetComponent<Image>().sprite);
            
            
            itemList.Add(itemName);
        
    }

    void TriggerPickUpPop(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
    }
    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount ==0)
            {
                return slot;
            }
        }

        return new GameObject();
    }

    public  bool CheckIfFull()
    {
        int counter = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount>0)
            {
                counter += 1;
            }

          
        }  
        if (counter == 15)
        {
            return true;
                         
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;
        for (var i =slotList.Count-1; i>=0;i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if(slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }
        ReCalculeList();
    }

    public void ReCalculeList()
    {
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                
                itemList.Add(result);
            }
        }
    }
}