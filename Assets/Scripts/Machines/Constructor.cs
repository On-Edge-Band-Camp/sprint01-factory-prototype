using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using static UnityEditor.Progress;

/*
---------------- BUGS TO FIX ----------------
1. if a recipe calls for the same object more then once, its currently has no way of knowing what its already counted (Kinda fixed with ratios, just compensate on input 2 by adding 1 more needed)
----------------------------------------------


*/

public class Constructor : Machine
{
    private GameItem[] materialNeeded;
    public GameItem finalProductName;

    private bool canCraft = false;
    private bool isCrafting = false;

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine() {

        //checks what we need to make this
        SearchForInputs();

        //checks if we have what we need
        inventoryCheck();

        //starts process once all needed mats are gathered
        if (canCraft && !isCrafting && finalProductName != null)
        { 
            StartCoroutine("process_timer");
            isCrafting = true;
        }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, GameItem item_type) {
        // imports item
        MachineInventory[item_type] += 1;
    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(GameItem item_type) {
        // delete component items on export
        for (int i = 0; i < materialNeeded.Length; i++)
        {
            //MachineInventory[materialNeeded[i]] -= 1 * gameManager.findItemByName(materialNeeded[i]).inputRatios[i];
        }
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process() {
        Debug.Log("Exporting!"); //Debug
        output_item(finalProductName);

        canCraft = false;
        isCrafting = false;
        
    }

    //Finds the recipe the inputs make
    private void SearchForInputs()
    {
        materialNeeded = null;
        //Checks the list of recipes for the inputs given, sets finalProduct to found recipe. Sets finalProduct to null if no recipe is found
        for (int i = 0; i < gameManager.items.Length; i++)
        {
            /*if (gameManager.items[i].Name == finalProductName)
            {
                materialNeeded = gameManager.items[i].inputNames;
                break;
            }*/
        }
        
        if (materialNeeded == null)
        {
            Debug.LogWarning("No Buildable path found. Check your imput for crafted meterial for typos");
        }
    }

    private void inventoryCheck()
    {
        //Seting up booleans for inventory check
        bool[] hasMateralsInInventory = new bool[materialNeeded.Length];
        for (int i = 0; i < hasMateralsInInventory.Length; i++)
        {
            hasMateralsInInventory[i] = false;
        }

        //checks if we have what we need in our inventory, sends error if it dose not reconize needed material
        for (int i = 0; i < materialNeeded.Length; i++)
        {
            try
            {
                /*if (MachineInventory[materialNeeded[i]] >= 1 * gameManager.findItemByName(materialNeeded[i]).inputRatios[i])
                {
                    hasMateralsInInventory[i] = true;
                }
                else
                {
                    hasMateralsInInventory[i] = false;
                }*/
            }
            catch
            {
                Debug.LogWarning("Needed material could not be found on constructor. Check the items spreadsheet for typos on input item names");
            }
        }

        //chekcs if the boolean is fully true
        for (int i = 0; i < hasMateralsInInventory.Length; i++)
        {
            canCraft = true;
            if (!hasMateralsInInventory[i])
            {
                canCraft = false;
                break;
            }
        }
    }
}
