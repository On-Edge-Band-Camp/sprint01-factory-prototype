using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSelectMenu : MonoBehaviour
{
    //Click bool to be set to true when button is clicked
    public static bool buttonClicked;
    //bool for when anywhere in the floating menu is clicked. Prevents menu deletion from clicking the menu itself.
    public static bool floatingMenuClicked;
    //Name string to be sent to placer script
    public static string selectionName;

    void Start()
    {
        
    }

    //The method in the button's inspector. Takes name from the inspector and sets it to a static string here for the 
    //  machine_selection method in placer script to use.
    public void SelectionName(string name)
    {
        buttonClicked = true;
        selectionName = name;
        if (name == "FloatingMenu")
        {
            floatingMenuClicked = true;
        }
    }

}
