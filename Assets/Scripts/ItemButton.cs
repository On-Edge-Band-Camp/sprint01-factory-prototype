using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public GameObject TargetSlot;
    public GameObject InvItem;
    Items ThisItem;
    Items ItemInSlot;
    // Start is called before the first frame update
    void Start()
    {
        ThisItem = gameObject.GetComponent<InventoryItem>().item;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Add a new recipete to a slot, clearing the old one.
    /// </summary>
    public void AddToSlot()
    {
        ClearSlot();
        //Due to Destroy running at the very end of a frame, items will attempt to stack on the items that are about to be destroyed
        //This is here to add a small delay so that new items would be added slightly after
        Invoke("AddNewItems",0.1f);
    }
    /// <summary>
    /// Clearing existing recipet on a slot
    /// </summary>
    public void ClearSlot()
    {
        foreach (Transform child in TargetSlot.transform)
        {
            var TryFindItem = child.GetComponent<InventoryItem>();
            if (TryFindItem != null)
            {
                TryFindItem.DestroySelf();
                TargetSlot.GetComponent<InventoryManager>().ClearAllItem();
                return;
            }
        }
    }

    /// <summary>
    /// Adding crafting item into recipet, and also its components.
    /// </summary>
    public void AddNewItems()
    {
        GameObject newItem = Instantiate(InvItem, TargetSlot.transform);
        newItem.GetComponent<InventoryItem>().InitializeItem(ThisItem);
        Debug.Log("New Recipet selected;");

        //Add items to empty component slots of the crafting menu
        foreach (Items item in ThisItem.Madeof)
        {
            Debug.Log("Adding:" + item);
            TargetSlot.GetComponent<InventoryManager>().AddItem(item);
        }
    }
}
