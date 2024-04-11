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
    public UIItem ItemSelectionUI;

    AudioSource StartSound;
    // Start is called before the first frame update
    void Start()
    {
        StartSound = GetComponent<AudioSource>();
        if (StartSound != null)
        {
            StartSound.Play();
        }
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
                newUIItem.machineDetails = this;
            }
        }
    }
    public void UpdateUIItem(GameItem item)
    {
        //Find all slots
        foreach (var uiitem in UIItems)
        {
            if (uiitem.Item != null)
            {
                Debug.Log(uiitem.Item);
                
                //If the UI item is the same item type
                if (uiitem.Item == item)
                {
                    //Update that item's count
                    uiitem.updateCount(machine.FindItemCount(item));
                    return;
                }
            }
        }
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
                //Find Empty UIItem slot
                foreach(var uiitem in UIItems)
                {
                    if(uiitem.Item == null)
                    {
                        uiitem.image.enabled = true;
                        uiitem.SetItem(item);
                        uiitem.updateCount(machine.MachineInventory[item]);
                        break;
                    }
                }
            }
        }
    }

    public void ClearItemSelection()
    {
        switch (machine.machine_type)
        {
            case machine_types.Collector:
                machine.GetComponent<Collector>().CollectingItem = null;
                break;
            case machine_types.Constructor:
                machine.GetComponent<Constructor>().finalProduct = null;
                break;
        }
    }

    public void EnableSubUI(GameObject subui)
    {
        subui.GetComponent<UIItemList>().machineDetails = this;
        subui.SetActive(true);      
    }
}
