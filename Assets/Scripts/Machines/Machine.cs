using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Machine: MonoBehaviour
{

    public Vector2Int grid_coord;

    public machine_types machine_type;

    public float process_time = 3;

    public int inventory_total; //FOR TESTING ONLY REMOVE

    //Stores references to all items in game for this machine
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    //Total amount of items able to be stored in inventory
    public int inventory_max = 10;

    //Input locations relative to center of machine, by default inputs from all 1x1 directions
    public Vector2Int[] input_directions = {
    
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    
    };

    //Output locations relative to center of machine, by default outputs to all directions
    public Vector2Int[] output_directions = {         
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    //When true, it means that the machine is currently performing a process, and doesn't need to be updated.
    public bool processing = false;

    //Activates when grid_handler sends and update call to this machine
    public abstract void update_machine();

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public abstract void handle_input(Vector2Int input_direction, string item_type);

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public abstract void handle_output(string item_type);

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public abstract void process();


    //FOR TESTING ONLY, REMOVE!!!!
    private void Update()
    {
        inventory_total = check_inventory_amount();
    }

    //idk why, but dictionaries need to be declared at runtime
    private void Start()
    {

        //Initialize inventory
        //REMOVE
        //PULL FROM SPREADSHEET INSTEAD
        inventory = new Dictionary<string, int>() {

            {"A", 0},
            {"B", 0},
            {"C", 0},
            {"AA", 0},
            {"AB", 0},
            {"AC", 0},
            {"BB", 0},
            {"BC", 0},
            {"CC", 0}

        };
    }


    //A timer for the process, only manipulated inventory in the process() function, custom to each machine.
    public IEnumerator process_timer()
    {
        processing = true;
        yield return new WaitForSeconds(process_time);
        process();
        processing = false;
    }


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

    //Check all output directions to see if a valid input is present
    public bool output_item(string item_type)
    {

        //All valid outputs are stored to this list, then chosen randomly
        List<GameObject> valid_outputs = new List<GameObject>();

        //Iterate through all output directions, if potential connection, add it to the valid_outputs list
        foreach (Vector2Int direction in output_directions) {

            GameObject target = check_output_connnection(direction);

            if (target != null)
            {
                valid_outputs.Add(target);
            }

        }

        //If list is empty, return false
        if (valid_outputs.Count == 0)
        {
            return false;
        }

        //Randomize output
        foreach (GameObject target in valid_outputs)
        {
            //Generate random point in outputs list
            int random_int = Random.Range(0, valid_outputs.Count);

            //check if random point successfullly handshaked
            if (output_check(valid_outputs[random_int], item_type))
            {
                return true;
            }

        }

        //If no output point is found, return false
        return false;
    }

    //Used to output an item to a nearby machine, calls input_item method on the inputting device.
    public bool output_check(GameObject target, string item_type)
    {

        //if machine is not a transporter, can only output to a transporter.
        if (machine_type != machine_types.Transporter)
        {
            //If target is a transporter
            if (target.GetComponent<Machine>().machine_type == machine_types.Transporter)
            {
                //Handshake with transporter
                if (target.GetComponent<Machine>().input_item(grid_coord, item_type))
                {
                    handle_output(item_type);
                    return true;
                }
            }

            //else return false
            return false;

        }

        //If machine is a transporter
        if (target.GetComponent<Machine>().input_item(grid_coord, item_type))
        {
            handle_output(item_type);
            return true;
        }

        return false;

    }

    //Checks if the inventory is currently full, if full return true
    public bool check_inventory_full()
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

    //FOR TESTING ONLY
    public int check_inventory_amount()
    {
        int inventory_count = 0;

        foreach (var item in inventory)
        {
            inventory_count += item.Value;
        }

        return inventory_count;
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



}
