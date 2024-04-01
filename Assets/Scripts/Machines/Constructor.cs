using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using static UnityEditor.Progress;

/*
---------------- BUGS TO FIX ----------------

----------------------------------------------


*/

public class Constructor : Machine
{
    public GameItem finalProduct;

    private bool isCrafting = false;

    private Dictionary<GameItem, int> neededMaterials = new Dictionary<GameItem, int>();

    //Activates when grid_handler sends and update call to this machine
    public override void update_machine() {

        if (finalProduct != null)
        {
            //checks what we need to make this
            SearchForInputs();

            //starts process once all needed mats are gathered
            if (inventoryCheck()) 
            {
                if(!isCrafting)
                {
                    //Debug.Log("Starting Work!");
                    StartCoroutine("process_timer");
                    isCrafting = true;
                }
                else
                {
                    ProgressTimer();
                }
            } 
 
        }
    }

    //Activates when an input is sent to this machine, can be used to handle unique outcomes depending on input location. Optional.
    public override void handle_input(Vector2Int input_direction, GameItem item_type) {
        // imports item
        MachineInventory[item_type] += 1;
        if(UI!=null)
        {
            UI.UpdateUIItem(item_type);
        }
    }

    //Activates when an output occurs, can be used to handle unique outcomes depending on the output location. Optional.
    public override void handle_output(GameItem item_type) {


    }

    //The process that occurs once the process timer is finished counting, for example the combining of two items and outputting them
    public override void process() {
        currentProcessInSec = 0;
        try
        {
            // delete component items on export
            foreach (GameItem item in neededMaterials.Keys)
            {
                MachineInventory[item] -= neededMaterials[item];
                try
                {
                    UI.UpdateUIItem(item);
                }
                catch
                {
                    
                }
            }
            output_item(finalProduct);

            if (UI != null)
            {
                UI.UpdateUIItem(finalProduct);
            }
        }
        catch
        {
            Debug.LogWarning("NO FINAL ITEM FOUND ON " + gameObject.name);
        }
        isCrafting = false;
        
    }

    /* <Summary>
     * turns MadeOf list from GameItem into a dictinary of needed items. This is done for ease of use in the rest of the code
     * <Summary>
     */
    private void SearchForInputs()
    {
        Dictionary<GameItem, int> batch = new Dictionary<GameItem, int>();

        for(int i = 0; i<finalProduct.MadeOf.Count; i++)
        {
            bool isInList = false;
            foreach (GameItem item in batch.Keys)
            {
                isInList = finalProduct.MadeOf[i] == item;
            }

            if (!isInList)
            {
                batch.Add(finalProduct.MadeOf[i], 1);
            }
            else
            {
                batch[finalProduct] += 1;
            }
        }

        if (batch.Count == 0)
        {
            Debug.LogWarning("No Buildable path found. Likly not a craftable object");
        }
        else
        {
            neededMaterials = batch;
        }
    }

    /* <Summary>
     * checks if the mechine has the materials needed to craft its product. Returns false if it does not, returns true if it does. 
     * Also returns true if the inventru doesn't know what the item is.
     * <Summary>
     */ 
    private bool inventoryCheck()
    {
        foreach(GameItem item in neededMaterials.Keys)
        {
            try
            {
                if (neededMaterials[item] > MachineInventory[item])
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        return true;
    }
}
