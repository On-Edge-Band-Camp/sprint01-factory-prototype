using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/*
---------------- BUGS TO FIX ----------------
1. if a recipe calls for the same object more then once, its currently has no way of knowing what its already counted
2. if an input is not valid, it has no way of knowing this untill it has already gone through the process timer
----------------------------------------------
*/

public class Constructor : Machine
{
    public string[] materialNeeded = new string[] {"A", "B"};

    private object finalProduct;
    private bool canCraft = false;
    private bool isCrafting = false;

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine() {

        //checks if it has the mats it needs
        foreach (var item in materialNeeded) 
        { 
            if(inventory[item] > 0) 
            {
                canCraft = true;
            }
            else
            {
                Debug.Log("Needs more " + item);
                canCraft = false;
                break;
            }
        }

        //starts process once all needed mats are gathered
        if (canCraft && !isCrafting)
        { 
            StartCoroutine("process_timer");
            isCrafting = true;
        }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, string item_type) {
        // imports item
        inventory[item_type] += 1;
    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(string item_type) {
        // delete component items on export
        for (int i = 0; i < materialNeeded.Length; i++)
        {
            inventory[materialNeeded[i]] -= 1;
        }
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process() {

        SearchForProduct();

        //Exports item
        if (finalProduct != null)
        {
            Debug.Log("Exporting!");
            output_item(finalProduct.ToString()); //DEBUG
        }

        Debug.Log("Finished crafting!"); //DEBUG
        canCraft = false;
        isCrafting = false;

    }

    //Finds the recipe the inputs make
    private void SearchForProduct()
    {
        finalProduct = null;

        //Checks the list of recipes for the inputs given, sets finalProduct to found recipe. Sets finalProduct to null if no recipe is found
        for (var i = 0; i < recipes.Count; i++)
        {
            Debug.Log("Attempting to find product...."); //DEBUG
            int part1Index = -1;
            int part2Index = -1;

            if (materialNeeded[0] == recipes[i]["Part1"].ToString())
            {
                part1Index = i;
            }
            else if (materialNeeded[0] == recipes[i]["Part2"].ToString())
            {
                part1Index = i;
            }

            if (materialNeeded[1] == recipes[i]["Part1"].ToString())
            {
                part2Index = i;
            }
            else if (materialNeeded[1] == recipes[i]["Part2"].ToString())
            {
                part2Index = i;
            }

            if (part1Index == part2Index && part1Index != -1)
            {
                finalProduct = recipes[i]["Product"];
                i = recipes.Count;
                Debug.Log("Product found! Making " + finalProduct); //DEBUG
            }
        }
    }
}
