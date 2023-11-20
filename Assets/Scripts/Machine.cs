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

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    //includes local output point
    public abstract void handle_output(Vector2Int output_direction, Vector2Int output_point, string item_type);



    //Called when an intem is being input to this machine.
    public bool input_item(Vector2Int output_machine_coord, string item_type)
    {

        if (check_inventory_full())
        {
            return false;
        }

        Vector2Int input = check_input_connection(output_machine_coord);

        if (input != new Vector2Int())
        {
            handle_input(input, item_type);
            return true;
        }

        return false;

    }

    //Used to output an item to a nearby machine, calls input_item method on the inputting device.
    bool output_item(Vector2Int output_direction, string item_type)
    {

        GameObject target = check_output_connnection(output_direction);

        if (target == new GameObject())
        {
            return false; 
        }

        if (target.GetComponent<Machine>().input_item(grid_coord, item_type))
        {
            handle_output(output_direction, item_type);
            return true;
        }

        return false;

    }

    //Used to output an item to a nearby machine, calls input_item method on the inputting device.
    //Output point refers to the local coord on the machine from which the output is occuring, meant for larger machines.
    bool output_item(Vector2Int output_direction, Vector2Int output_point, string item_type)
    {
        GameObject target = check_output_connnection(output_direction, output_point);

        if (target == new GameObject())
        {
            return false;
        }

        if (target.GetComponent<Machine>().input_item(grid_coord + output_point, item_type))
        {
            inventory[item_type] -= 1;
            handle_output(output_direction, output_point, item_type);
            return true;
        }

        return false;
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

    //Checks if the output direction given connects to any other gameobjects in the grid, and returns the gameobject, if empty returns empty gameobject
    GameObject check_output_connnection(Vector2Int output_direction)
    {
        return GridController.grid[grid_coord.x + output_direction.x, grid_coord.y + output_direction.y];
    }


    //Checks if the output direction given connects to any other gameobjects in the grid, and returns the gameobject, if empty returns empty gameobject
    //includes local output point
    GameObject check_output_connnection(Vector2Int output_direction, Vector2Int output_point)
    { 
        return GridController.grid[grid_coord.x + output_point.x + output_direction.x, grid_coord.y + output_point.y + output_direction.y];
    }



}
