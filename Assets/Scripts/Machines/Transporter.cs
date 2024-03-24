using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : Machine
{

    GameItem held_item;
    public ItemDisplayer itemDisplayer;

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine()
    {
        if (held_item != null)
        {
            ProgressTimer();
            if (!processing && check_inventory_full())
            {
                StartCoroutine("process_timer");
            }
        }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, GameItem item_type) {
        held_item = item_type;
        MachineInventory[item_type] += 1;
        if (itemDisplayer != null)
        {
            itemDisplayer.updateSprite(held_item);
        }
        if (UI != null)
        {
            UI.UpdateUIItem(item_type);
        }
    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(GameItem item_type)
    {
        if (MachineInventory[item_type] > 0)
        {
            MachineInventory[item_type] -= 1;
            held_item = null;
            if (itemDisplayer != null)
            {
                itemDisplayer.updateSprite(held_item);
            }
            if (UI != null)
            {
                UI.UpdateUIItem(item_type);
            }
        }
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process()
    {
        if (held_item != null)
        {
            output_item(held_item);
        }
    }

}
