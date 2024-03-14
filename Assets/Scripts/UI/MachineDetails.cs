using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MachineDetails : MonoBehaviour
{
    public Machine machine;
    public List<Slot> slots;

    public GameObject emptyUIItem;
    public SOItem testItem;

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
        //Initialize all Item Slots
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot newSlot = transform.GetChild(i).GetComponent<Slot>();
            if (newSlot != null)
            {
                slots.Add(newSlot);
                GameObject newUIItem = Instantiate(emptyUIItem, newSlot.transform);
                newUIItem.GetComponent<UIItem>().Hide();
            }
        }
    }
    public void UpdateUIItem(GameItem item)
    {
        foreach (Slot slot in slots)
        {
            UIItem uiitem = slot.GetComponentInChildren<UIItem>();
            if (uiitem.Item != null)
            {
                if (uiitem.Item == item)
                {               
                    uiitem.updateCount(machine.FindItemCount(item));
                    return;
                }
            }
        }
        //If no matching item exist in the current UI, find a empty slot and create one
        foreach (Slot slot in slots)
        {
            UIItem uiitem = slot.GetComponentInChildren<UIItem>();
            if (uiitem.Item == null)
            {
                uiitem.SetItem(item);
                uiitem.updateCount(machine.FindItemCount(item));
                return;
            }
        }
    }

    public void InputNewInventory(Machine machine)
    {
        InitializeSlots();
        foreach (Slot slot in slots)
        {
            slot.GetComponentInChildren<UIItem>().Clear();
        }

        foreach(var item in machine.MachineInventory.Keys)
        {
            if (machine.MachineInventory[item] > 0)
            {
                Debug.Log($"{item} : {machine.MachineInventory[item]}");
                UpdateUIItem(item);
            }
        }
    }
}
