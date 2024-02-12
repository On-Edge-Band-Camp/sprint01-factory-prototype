using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public GameObject TargetSlot;
    public GameObject InvItem;
    Items ThisItem;
    // Start is called before the first frame update
    void Start()
    {
        ThisItem = gameObject.GetComponent<InventoryItem>().item;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToSlot()
    {
        //Try to find an exsiting item in target slot
        var currentItemInSlot = TargetSlot.GetComponentInChildren<InventoryItem>();
        if (currentItemInSlot != null)
        {
            //Clear Exsiting slots
            Destroy(currentItemInSlot.gameObject);
            TargetSlot.GetComponent<InventoryManager>().ClearAllItem();
        }

        GameObject newItem = Instantiate(InvItem, TargetSlot.transform);

        newItem.GetComponent<InventoryItem>().InitializeItem(ThisItem);

        //Add items to empty component slots of the crafting menu
        foreach(Items item in ThisItem.Madeof)
        {
            var Inv = TargetSlot.GetComponent<InventoryManager>();
            if (Inv != null)
            {
                Inv.AddItem(item);
            }
        }
    }
}
