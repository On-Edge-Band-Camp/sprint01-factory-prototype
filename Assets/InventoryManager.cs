using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] InventorySlots;
    public GameObject InvItem;
    /// <summary>
    /// Add a item into a machine's inventory
    /// </summary>
    public void AddItem(Items InputItem)
    {
        if (!StackItem(InputItem))
        {
            //Find an empty slot by looping thourgh all the slots in the inventory
            foreach (GameObject slot in InventorySlots)
            {
                Debug.Log("Finding empty slot");
                InventoryItem ItemInSlot = slot.GetComponentInChildren<InventoryItem>();
                //When there is not a child(Item) in a slot, this is a empty slot
                if (ItemInSlot == null)
                {
                    SpawnNewItem(InputItem, slot);
                    return;
                }
            }
            Debug.Log("NO EMPTY SLOT LEFT! IS MACHINE FULL??");
            return;
        }
    }

    public void RemoveItem(Items InputItem)
    {
        //Find a slot with the same object in it to remove;
        foreach (GameObject slot in InventorySlots)
        {
            InventoryItem ItemInSlot = slot.GetComponentInChildren<InventoryItem>();
            //if there is a same item, add its count and return
            if (ItemInSlot != null && ItemInSlot.item == InputItem)
            {
                ItemInSlot.count--;
                ItemInSlot.RefreshCount();
                if (ItemInSlot.count <= 0)
                {
                    Destroy(ItemInSlot.gameObject);
                }
                Debug.Log("item Removed");
                return;
            }
            Debug.Log("No Matching items found");
            return;
        }
    }

    /// <summary>
    /// try to find same item and stack it, and return if stack is successful or not
    /// </summary>
    public bool StackItem(Items InputItem)
    {
        //Find a slot with the same object in it to stack;
        foreach (GameObject slot in InventorySlots)
        {
            InventoryItem ItemInSlot = slot.GetComponentInChildren<InventoryItem>();
            //if there is a same item, add its count and return
            if (ItemInSlot != null && ItemInSlot.item == InputItem && ItemInSlot.count < InputItem.MaxStack)
            {
                ItemInSlot.count++;
                ItemInSlot.RefreshCount();
                Debug.Log("Stacked");
                return true;
            }
        }
        Debug.Log("Item stacked to max or failed to stack");
        return false;
    }

    /// <summary>
    /// Spawn a new Item in a empty slot
    /// </summary>
    public void SpawnNewItem(Items ItemToSpawn, GameObject Slot)
    {
        //Spawn a new item as a game object insided the selected empty slot
        GameObject newItem = Instantiate(InvItem, Slot.transform);
        //Initialize all the information needed to display this item in the UI
        newItem.GetComponent<InventoryItem>().InitializeItem(ItemToSpawn);
    }

    //Find all slots and count how many items of the same type there is.
    public int CountTotal(Items ItemToCount)
    {
        int TotalItem = 0;
        //Find a slot with the same object in it;
        foreach (GameObject slot in InventorySlots)
        {
            InventoryItem ItemInSlot = slot.GetComponentInChildren<InventoryItem>();
            //if there is a same item, add its count to the total
            if (ItemInSlot != null && ItemInSlot.item == ItemToCount)
            {
                TotalItem += ItemInSlot.count;
            }
        }
        return TotalItem;
    }

    public void ClearAllItem()
    {
        foreach(GameObject slot in InventorySlots)
        {
            Destroy(slot.transform.GetChild(0).gameObject);
        }
    }
}
