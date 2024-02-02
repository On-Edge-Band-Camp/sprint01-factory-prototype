using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Constructor : Machine
{
    public object[] materialNeeded = new string[] {"A", "B"};

    private object finalProduct;
    private bool canCraft = false;

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine() {

        //checks if it has the mats it needs
        for(int i = 0; i < materialNeeded.Length; i++)
        {
            if(inventory[materialNeeded[i].ToString()] > 0)
            {
                canCraft = true;
            }
            else
            {
                canCraft = false;
                i = materialNeeded.Length;
            }
        }

        //starts process once all need mats are gathered
        if (canCraft)
        { 
            StartCoroutine("process_timer");
            canCraft = false;
        }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, string item_type) {
        inventory[item_type] += 1;
    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(string item_type) {
        // delete component items
        for (int i = 0; i < materialNeeded.Length; i++)
        {
            inventory[item_type] -= 1;
        }
    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process() {

        finalProduct = null;

        //Finds the item that is being made
        for (var i = 0; i < recipes.Count; i++)
        {

            int part1Index = -1;
            int part2Index = -1;

            if (materialNeeded[0] == recipes[i]["Part1"])
            {
                part1Index = i;
            }
            else if (materialNeeded[0] == recipes[i]["Part2"])
            {
                part1Index = i;
            }

            if (materialNeeded[1] == recipes[i]["Part1"])
            {
                part2Index = i;
            }
            else if (materialNeeded[1] == recipes[i]["Part2"])
            {
                part2Index = i;
            }

            if (part1Index == part2Index && part1Index != -1)
            {
                finalProduct = recipes[i]["Product"];
                i = recipes.Count;
            }
        }

        //Exports item
        if (finalProduct != null)
        {
            Debug.Log(output_item(finalProduct.ToString()));
        }


    }
}
