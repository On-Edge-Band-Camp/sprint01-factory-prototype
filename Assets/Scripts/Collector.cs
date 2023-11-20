using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class collector : Machine
{

    public string collected_item = "wood";

    public Vector2Int output_direction = new Vector2Int(1,0);

    public collector(Vector2Int grid_coord) : base(grid_coord) {

        machine_type = "collector";
    
    }

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine() {

        if (!processing) { process_timer(); }

        if (inventory[collected_item] > 0)
        {
            output_item(output_direction, collected_item);
        }

    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, string item_type) { }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(Vector2Int output_direction, string item_type) {

        if (inventory[item_type] > 0)
        {
            inventory[item_type] -= 1;
        }
    
    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    //includes local output point
    public override void handle_output(Vector2Int output_direction, Vector2Int output_point, string item_type) {

        if (inventory[item_type] > 0)
        {
            inventory[item_type] -= 1;
        }

    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process() {

        if (!check_inventory_full())
        {
            inventory[collected_item] += 1;
        } else
        {
            Debug.Log(grid_coord + ": Inventory Full");
        }
    
    }

}
