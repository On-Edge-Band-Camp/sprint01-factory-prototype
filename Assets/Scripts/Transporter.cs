using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : Machine
{

    string held_item;

    public Transporter(Vector2Int grid_coord) : base(grid_coord)
    {

        machine_type = "transporter";
        inventory_max = 1;
        input_directions = new Vector2Int[] {new Vector2Int(-1,0)};
        process_time = 1;

    }

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine()
    {

        if (!processing && check_inventory_full() && held_item != null)
        {
            process_timer();
        }

    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, string item_type) {

        held_item = item_type;
        inventory[item_type] += 1;

    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(Vector2Int output_direction, string item_type)
    {

        if (inventory[item_type] > 0)
        {
            inventory[item_type] -= 1;
            held_item = null;
        }

    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    //includes local output point
    public override void handle_output(Vector2Int output_direction, Vector2Int output_point, string item_type)
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
        output_item(new Vector2Int(1, 0), held_item);
    }

}
