using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collector : Machine
{
    public GameItem CollectingItem;
    public int CollectAmountPerTick;

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine() {

        if (processing)
        {
            ProgressTimer();
        }

        if (CollectingItem != null)
        {
            if (!processing && !check_inventory_full()) { StartCoroutine("process_timer"); }

            if (check_inventory_amount() != 0)
            {
                output_item(CollectingItem);
            }
        }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, GameItem item_type) { }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(GameItem item_type) {       
        MachineInventory[item_type] -= 1;
        if (UI != null)
        {
            UI.UpdateUIItem(item_type);
        }
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process() {

        currentProcessInSec = 0;

        if (CollectingItem != null && !check_inventory_full())
        {
            AddItem(CollectingItem, CollectAmountPerTick);
        }
        else if(CollectingItem == null)
        {
            Debug.LogWarning("No item is assigned to Collector");
        }
    }

}
