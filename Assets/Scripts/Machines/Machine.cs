using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;



public abstract class Machine: MonoBehaviour
{
    public int energyCost;
    public GameObject UIToUse;

    public Vector2Int grid_coord;

    public machine_types machine_type;

    public float process_time = 3;
    protected float currentProcessInSec;
    public float currentProcessInPercent;

    public int inventory_total; //FOR TESTING ONLY REMOVE

    //Total amount of items able to be stored in inventory OUTDATED
    public int inventory_max = 20;

    public GameManager gameManager;
    //This is the child gameobject that holds all the particles in this object
    public GameObject particleMaster;

    public Dictionary<GameItem, int> MachineInventory = new Dictionary<GameItem, int>();
    public MachineDetails UI;

    public SpriteRenderer HighLight;

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

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional. OUTDATED
    public abstract void handle_input(Vector2Int input_direction, GameItem item_type);

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional. OUTDATED
    public abstract void handle_output(GameItem item_type);

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public abstract void process();

    //FOR TESTING ONLY, REMOVE!!!!
    private void Update()
    {
        inventory_total = check_inventory_amount();
        if (particleMaster != null)
        {
            particleMaster.SetActive(processing);
        }
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        DisableHighlight();
    }

    //idk why, but dictionaries need to be declared at runtime
    private void Start()
    {
        Debug.Log("Initializing Inventory");
        //Initialize Inventory with all items in game.
        foreach(GameItem item in gameManager.AllGameItems)
        {
            MachineInventory.Add(item, 0);
            Debug.Log($"{item}: {MachineInventory[item]}");
        }
        

        if (particleMaster != null)
        {
            particleMaster.SetActive(false);
        }
    }

    //A timer for the process, only manipulated inventory in the process() function, custom to each machine.
    public IEnumerator process_timer()
    {
        processing = true;
        yield return new WaitForSeconds(process_time);
        process();
        processing = false;
    }

    /// <summary>
    /// Progress Timmer used for UIs
    /// </summary>
    public void ProgressTimer()
    {
        currentProcessInSec += Time.deltaTime;
        currentProcessInPercent = currentProcessInSec / process_time;
    }

    //Called when an intem is being input to this machine.
    public bool input_item(Vector2Int output_machine_coord, GameItem item_type)
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
    
    public bool output_item(GameItem item_type, [Optional] int specificOutput)
    {
        List<GameObject> valid_outputs = validify_outputs();

        //If list is empty, return false
        if (valid_outputs.Count == 0)
        {
            Debug.LogWarning("NO VALID OUTPUT ON: " + gameObject.name);
            return false;
        }


        //Randomize output if no specific output is given
        if (specificOutput == 0)
        {
            foreach (GameObject target in valid_outputs)
            {
                //Generate random point in outputs list
                int random_int = UnityEngine.Random.Range(0, valid_outputs.Count);

                //check if random point successfullly handshaked
                if (output_check(valid_outputs[random_int], item_type))
                {
                    return true;
                }
            }
            Debug.LogWarning("OUTPUT CHECK FAILED ON: " + gameObject.name);
        }
        else //Or uses specific output
        {
            specificOutput--;
            if (output_check(valid_outputs[specificOutput], item_type))
            {
                return true;
            }
            Debug.LogWarning("OUTPUT CHECK FAILED ON: " + gameObject.name);
        }


        //If no output point is found, return false
        return false;
    }

    //Iterate through all output directions, if potential connection, add it to the valid_outputs list
    public List<GameObject> validify_outputs()
    {
        List<GameObject> valid_outputs = new List<GameObject>();

        foreach (Vector2Int direction in output_directions)
        {
            bool validInputOnTarget = false;
            GameObject target = check_output_connnection(direction);

            try
            {
                validInputOnTarget = target.GetComponent<Machine>().check_input_connection(grid_coord) != new Vector2Int();
            }
            catch
            {

            }


            if (target != null && validInputOnTarget)
            {
                valid_outputs.Add(target);
            }

        }

        return valid_outputs;
    }

    //Used to output an item to a nearby machine, calls input_item method on the inputting device.
    public bool output_check(GameObject target, GameItem item_type)
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

        foreach(var item in MachineInventory)
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

        foreach (var item in MachineInventory)
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

    /// <summary>
    /// Add x amount of an item to the machine's inventory
    /// </summary>
    public void AddItem(GameItem item, int count)
    {
        //Iterate through all items
        foreach (var key in MachineInventory.Keys)
        {
            //If a item is the matching item, that is the one we wish to add
            if (key == item)
            {
                //Add count to its value
                MachineInventory[key] += count;
                if (UI != null)
                {
                    UI.UpdateUIItem(item);
                }
                //Debug.Log($"Added {count} of {key.ItemName}, now have {MachineInventory[key]} {key.ItemName}s");
                return;
            }
        }
    }

    /// <summary>
    /// Returns how many items of the type SOItem is in the inventory
    /// </summary>
    /// <param name="soitem"></param>
    /// <returns></returns>
    public int FindItemCount(GameItem soitem)
    {
        //Iterate through all items
        foreach (var key in MachineInventory.Keys)
        {
            if (key == soitem)
            {
                return MachineInventory[key];
            }
        }
        return 0;
    }

    private void OnMouseEnter()
    {
        EnableHighlight();
    }

    private void OnMouseExit()
    {
        DisableHighlight();
    }

    public void EnableHighlight()
    {
        Debug.Log("Enable Highlight");
        GetComponent<Renderer>().material.SetFloat("_Thickness", 0.025f);
    }

    public void DisableHighlight()
    {
        GetComponent<Renderer>().material.SetFloat("_Thickness", 0f);
    }
}
