using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Machine
{

    Vector2Int grid_coord;

    public string machine_type;

    //Tester entries
    public Dictionary<string, int> inventory = new Dictionary<string, int>() {

        {"Wood", 0},
        {"Stone", 0},
        {"Metal", 0}
    
    };

    //Total amount of items able to be stored in inventory
    public int inventory_max = 10;

    //Input locations relative to center of machine
    public Vector2Int[] input_directions;

    //If this machine has been updated
    bool updated = false;

    //Class Contructor
    Machine(Vector2Int center_grid_coord) {grid_coord = center_grid_coord;}



    //Activates when grid_handler sends and update call to this machine.
    public abstract void update_machine();

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public abstract void handle_input(Vector2Int input_direction, string item_type);

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public abstract void handle_output(Vector2Int output_direction, string item_type);



    //Called when an intem is being input to this machine.
    public bool input_item(Vector2Int output_machine_coord, string item_type)
    {

        if (!check_inventory_full())
        {

        }

        return false;

    }

    //Used to output an item to a nearby machine, calls input_item method on the inputting device.
    void output_item(Vector2Int output_direction, string item_type)
    {

    }

    //Used to output an item to a nearby machine, calls input_item method on the inputting device.
    //Output point refers to the local coord on the machine from which the output is occuring, meant for larger machines.
    void output_item(Vector2Int output_direction, Vector2Int output_point, string item_type)
    {
        
    }

    //Checks if the inventory is currently full, if full return true
    bool check_inventory_full()
    {
        int inventory_count = 0;

        foreach(var item in inventory)
        {
            inventory_count += item.Value;
        }

        if (inventory_count >= inventory_max)
        {
            return true;
        }

        return false;
    }

    //Checks if any of the inputs connect to the outputting machine, if one does connect, returns the value of that input point, if no matching point is found, returns empty vector
    Vector2Int check_input_connection(Vector2Int coord)
    {

        foreach (Vector2Int input in input_directions)
        {
            if(grid_coord + input == coord) { return input; }
        }

        return new Vector2Int();

    }

    //Checks if the output direction given connects to any other gameobjects in the grid, and returns the gameobject
    GameObject check_output_connnection(Vector2Int output_direction, Vector2Int output_point)
    {
        return new GameObject();
    }



}
