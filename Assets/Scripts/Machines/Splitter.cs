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
    public override void handle_input(Vector2Int input_direction, string item_type) {

        inventory[item_type] += 1;

    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(string item_type)
    {
        inventory[item_type] -= 1;
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process()
    {

        foreach (var item in inventory)
        {
            if (item.Value > 0)
            {
                Debug.Log(output_item(item.Key));
                break;
            }

        }

    }

    #region Zephyr's Variant Methods
    public override void handle_input(Vector2Int input_direction, Items item_type)
    {

    }

    public override void handle_output(Items item_type)
    {

    }
    #endregion
}
