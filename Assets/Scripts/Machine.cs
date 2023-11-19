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
        return false;
    }

    //Used to output an item to a nearby machine, calls input_item method on the inputting device.
    void output_item(Vector2Int output_direction, string item_type)
    {
        
    }



}
