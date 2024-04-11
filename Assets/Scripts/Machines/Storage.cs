using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Machine
{

    //Activates when grid_handler sends and update call to this machine
    public static int winningItemAmount;
    public override void update_machine()
    {
        foreach(GameItem item in MachineInventory.Keys)
        {
            if (MachineInventory[item] > 0)
            {
                //Debug.Log($"{item}  {MachineInventory[item]}");
                if (!processing) { StartCoroutine("process_timer"); }
                return;
            }
        }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, GameItem item_type) {
        
        MachineInventory[item_type] += 1;
        if(item_type.name == GridController.winningItem1)
        {
            winningItemAmount++;
        }
        if (UI != null)
        {
            UI.UpdateUIItem(item_type);
        }
    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(GameItem item_type)
    {
        MachineInventory[item_type] -= 1;
        if (UI != null)
        {
            UI.UpdateUIItem(item_type);
        }
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process()
    {
        foreach (var item in MachineInventory.Keys)
        {
            if (MachineInventory[item] > 0)
            {
                output_item(item);
                return;
            }
        }
    }
}
