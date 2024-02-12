using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : Machine
{

    string held_item;

    public Items HoldingItem;

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine()
    {

        if (!processing && check_inventory_full() && held_item != null)
        {
            StartCoroutine("process_timer");
        }

    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, string item_type) {

        held_item = item_type;
        inventory[item_type] += 1;

    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(string item_type)
    {

        if (inventory[item_type] > 0)
        {
            inventory[item_type] -= 1;
            held_item = null;
        }

    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process()
    {
        if (held_item != null)
        {
            output_item(HoldingItem);
            output_item(held_item);
        }
    }

    #region Zephyr's Variant Methods
    public override void handle_input(Vector2Int input_direction, Items item_type)
    {
        HoldingItem = item_type;
        Inventory.AddItem(item_type);
    }

    public override void handle_output(Items item_type)
    {
        Inventory.RemoveItem(item_type);
        HoldingItem = null;
    }
    #endregion
}
