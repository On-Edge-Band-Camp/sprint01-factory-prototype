using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineDetails : MonoBehaviour
{
    public Machine machine;
    public List<Slot> slots;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot newSlot = transform.GetChild(i).GetComponent<Slot>();
            if(newSlot!=null)
            {
                slots.Add(newSlot);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameItem item in machine.MachineInventory.Keys)
        {
            if (machine.MachineInventory[item] > 0)
            {
                Debug.Log(item.ItemName);
            }
        }
    }
}
