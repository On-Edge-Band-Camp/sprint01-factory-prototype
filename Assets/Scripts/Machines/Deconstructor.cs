using System.Collections;
using System.Collections.Generic;
//using System.Runtime.ConstrainedExecution;
using UnityEngine;
//using static UnityEditor.Progress;

public class Deconstructor : Machine
{

    private string neededMeterial;
    //private object[] finalProducts = null;
    
    private bool canDeconstruct = false;
    private bool isDeconstructing = false;

    public override void update_machine()
    {
        if (inventory[neededMeterial] > 0)
        {
            canDeconstruct = true;
        }

        if (canDeconstruct && !isDeconstructing)
        {
            StartCoroutine("process_timer");
            isDeconstructing = true;
        }
    }

    public override void handle_input(Vector2Int input_direction, string item_type)
    {
        throw new System.NotImplementedException();
    }

    public override void handle_output(string item_type)
    {
        throw new System.NotImplementedException();
    }

    public override void process()
    {
        throw new System.NotImplementedException();
    }

    private void SearchForRecipe()
    {
        //finalProducts = null;
        for (var i = 0; i < recipes.Count; i++)
        {
            /*if (neededMeterial == recipes[i]["Product"].ToString)
            {
                finalProducts = new object[] { recipes[i]["Part1"], recipes[i]["Part2"] };
            }*/
        }
    }
}
