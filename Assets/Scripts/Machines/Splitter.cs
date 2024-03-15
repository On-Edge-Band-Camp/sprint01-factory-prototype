using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Machine
{

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine()
    {
        if (!processing) { StartCoroutine("process_timer"); }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, GameItem item_type) {

        MachineInventory[item_type] += 1;

    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(GameItem item_type)
    {
        MachineInventory[item_type] -= 1;
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process()
    {

        foreach (var item in MachineInventory)
        {
            if (item.Value > 0)
            {
                Debug.Log(output_item(item.Key));
                break;
            }

        }

    }
}
