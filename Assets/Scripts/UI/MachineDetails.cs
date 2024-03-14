using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MachineDetails : MonoBehaviour
{
    public Machine machine;
    public List<Slot> slots;
    public List<UIItem> UIItems;

    public GameObject emptyUIItem;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSlots();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitializeSlots()
    {
        slots.Clear();
        UIItems.Clear();

        //Initialize all Item Slots
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot newSlot = transform.GetChild(i).GetComponent<Slot>();
            if (newSlot != null)
            {
                slots.Add(newSlot);
                var newUIItem = newSlot.GetComponentInChildren<UIItem>();
                UIItems.Add(newUIItem.GetComponentInChildren<UIItem>());
            }
        }
    }
    public void UpdateUIItem(GameItem item)
    {
        Debug.Log("Updating Items");
        //Find all slots
        foreach (var uiitem in UIItems)
        {
            if (uiitem.Item != null)
            {
                Debug.Log(uiitem.Item);
                //If the UI item is the same item type
                if (uiitem.Item == item)
                {
                    Debug.Log("Found same item");
                    //Update that item's count
                    uiitem.updateCount(machine.FindItemCount(item));
                    return;
                }
            }
        }
        Debug.Log("No same item found, adding new instead");
        //If no matching item exist in the current UI, find a empty slot and create one
        foreach (UIItem uiitem in UIItems)
        {
            if (uiitem.Item == null)
            {
                uiitem.SetItem(item);
                uiitem.updateCount(machine.FindItemCount(item));
                return;
            }
        }
    }

    public void InputNewInventory()
    {
        InitializeSlots();
        foreach (UIItem uiitem in UIItems)
        {
            uiitem.Clear();
        }

        foreach(var item in machine.MachineInventory.Keys)
        {
            if (machine.MachineInventory[item] > 0)
            {
                foreach(var uiitem in UIItems)
                {
                    if(uiitem.Item == null)
                    {
                        uiitem.image.enabled = true;
                        uiitem.SetItem(item);
                        uiitem.updateCount(machine.FindItemCount(item));
                    }
                    return;
                }
            }
        }
    }
}
