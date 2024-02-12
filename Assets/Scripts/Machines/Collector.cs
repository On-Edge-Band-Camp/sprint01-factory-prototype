using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collector : Machine
{

    public string collected_item = "Test";
    public Items CollectingItem;
    //Activates when grid_handler sends and update call to this machine
    public override void update_machine() {

        if (!processing && !check_inventory_full()) { StartCoroutine("process_timer");}

        if (check_inventory_amount() != 0)
        {
            output_item(CollectingItem);
            output_item(collected_item);
        }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, string item_type) { }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(string item_type) {

        
        inventory[item_type] -= 1;
    
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process() {

        if (!check_inventory_full())
        {
            inventory[collected_item] += 1;
            CollectItem(CollectingItem);
        } 
    }

    void CollectItem(Items item)
    {
        if (item != null)
        {
            Inventory.AddItem(item);
        }
    }

    #region Zephyr's Variant Methods
    public override void handle_input(Vector2Int input_direction, Items item_type)
    {

    }

    public override void handle_output(Items item_type)
    {
        Inventory.RemoveItem(item_type);
    }
    #endregion
}
