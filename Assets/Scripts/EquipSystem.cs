
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
 
public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }
 
    // -- UI -- //
    public GameObject quickSlotsPanel;
 
    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;
    public GameObject toolHolder;
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
 
 
    private void Start()
    {
        PopulateSlotList();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuicSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuicSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuicSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuicSlot(4);
        }
    }

    private void SelectQuicSlot(int number)
    {
        if (checkIfFull(number)==true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;

                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
            
                selectedItem = getSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;
                
                Debug.Log(selectedItem.name);
               SetEquippedModel(selectedItem);
                
                //changing the color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color=Color.gray;
            
                }
                Text toBeChanger = numbersHolder.transform.Find("number" + number).transform.Find("Text").GetComponent<Text>();
                toBeChanger.color = Color.white;
            }
            else
                //trying to select the same slot
            {
                selectedNumber = -1;
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color=Color.gray;
            
                }
            }
        }
        
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
       string selectedItemName = selectedItem.name.Replace("(Clone)", "");
       GameObject itemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(0.304f, 0.87f, 0.635f),Quaternion.Euler(-1.973f, 83.094f, 6.819f));
       itemModel.transform.SetParent(toolHolder.transform,false);
    }

    private GameObject getSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    private bool checkIfFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber-1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }
 
    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object, set different parent from slot to quickSlot
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemList.Add(cleanName);
 
        InventorySystem.Instance.ReCalculeList();
 
    }
 
 
    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }
 
    public bool CheckIfFull()
    {
 
        int counter = 0;
 
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }
 
        if (counter == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}