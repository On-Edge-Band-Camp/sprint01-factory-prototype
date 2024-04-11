using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MachineSelectMenu : MonoBehaviour
{
    //Click bool to be set to true when button is clicked
    public static bool buttonClicked;
    //bool for when anywhere in the floating menu is clicked. Prevents menu deletion from clicking the menu itself.
    public static bool floatingMenuClicked;
    //Name string to be sent to placer script
    public static string selectionName;

    public static GameObject uiObject;

    void Start()
    {
        
    }

    //The method in the button's inspector. Takes name from the inspector and sets it to a static string here for the 
    //  machine_selection method in placer script to use.
    public void SelectionName(string name)
    {
        buttonClicked = true;
        selectionName = name;
        uiObject = this.gameObject;
        if (name == "FloatingMenu")
        {
            floatingMenuClicked = true;
        }
    }

    public void DontCloseOnClick()
    {
        buttonClicked = true;
        floatingMenuClicked = true;
    }

    public void ThisObject()
    {
        //uiObject = this.gameObject;
    }
}
