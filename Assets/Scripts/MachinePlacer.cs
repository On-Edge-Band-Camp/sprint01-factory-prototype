using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePlacer : MonoBehaviour
{
    //Gameobject set to the currently selected machine. Follows the mouse until set on the grid.
    GameObject current_selection;

    GridController grid_control;

    Vector2 mousePos;

    //World space position on the grid for the selected machine to snap to.
    Vector2 new_world_pos;

    //Index of the abstract grid to set the machine to when placed.
    Vector2Int new_grid_coord;

    //All machine prefabs
    public GameObject collector_prefab;
    public GameObject transporter_left_prefab;
    public GameObject transporter_right_prefab;
    public GameObject transporter_up_prefab;
    public GameObject transporter_down_prefab;
    public GameObject storage_prefab;
    public GameObject splitter_prefab;

    void Start()
    {
        grid_control = GetComponent<GridController>();
    }


    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        machine_select();
        machine_placer();
        //setting menu's buttonclick to false after click is resolved.
        MachineSelectMenu.buttonClicked = false;
    }


    //Takes static bool clicked and static string name from MachineSelectMenu script when button is pressed.
    //If a machine is currently selected and button is pressed again, destroy current selection
    void machine_select()
    {
        if (MachineSelectMenu.buttonClicked == true) 
        { 
            //Collector Selection
            if (MachineSelectMenu.selectionName == "Collector")
            {
                Destroy(current_selection);
                current_selection = Instantiate(collector_prefab, mousePos, Quaternion.identity);
            }

            //Transporter Selection
            if (MachineSelectMenu.selectionName == "TransporterLeft")
            {
                Destroy(current_selection);
                current_selection = Instantiate(transporter_left_prefab, mousePos, Quaternion.identity);
            }

            if (MachineSelectMenu.selectionName == "TransporterRight")
            {
                Destroy(current_selection);
                current_selection = Instantiate(transporter_right_prefab, mousePos, Quaternion.identity);
            }

            if (MachineSelectMenu.selectionName == "TransporterUp")
            {
                Destroy(current_selection);
                current_selection = Instantiate(transporter_up_prefab, mousePos, Quaternion.identity);
            }

            if (MachineSelectMenu.selectionName == "TransporterDown")
            {
                Destroy(current_selection);
                current_selection = Instantiate(transporter_down_prefab, mousePos, Quaternion.identity);
            }

            //Storage Selection
            if (MachineSelectMenu.selectionName == "Storage")
            {
                Destroy(current_selection);
                current_selection = Instantiate(storage_prefab, mousePos, Quaternion.identity);
            }

            //Splitter Selection
            if (MachineSelectMenu.selectionName == "Splitter")
            {
                Destroy(current_selection);
                current_selection = Instantiate(splitter_prefab, mousePos, Quaternion.identity);
            }
        }
    }

    //Method for snapping a selected, hovering machine to the grid as it moves with the mouse position.
    //This method calls the set_machine() method.
    void machine_placer()
    {
        //The distance between the mouse position and the currently closest cell position.
        //Updates to the shortest distance through iterative checking with every cell position.  
        float closestDistance = 0;

        //Only if a machine is currently selected,
        if (current_selection != null)
        {

            for (int i = 0; i < grid_control.grid_dimensions.y; i++)
            {
                for (int j = 0; j < grid_control.grid_dimensions.x; j++)
                {

                    //If the current index of the abstract grid is occupied, skip iteration.
                    if (GridController.grid[j, grid_control.grid_dimensions.y - (i + 1)] != null)
                        continue;
                    
                    //Distance between the mouse position and current worldspace cell position
                    float distance = Vector2.Distance(mousePos, grid_control.odd_worldspace_center()[j, i]);

                    //At iteration [0,0], initiallize closest distance and new position to the current cell parameters
                    if (i == 0 && j == 0)
                    {
                        closestDistance = distance;
                        new_world_pos = grid_control.odd_worldspace_center()[j, i];
                    }

                    //Through iteration, if a distance is found that is less than the current closest distance,
                    //set closest distance to this distance and world_pos and grid_coord to this location's parameters.
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        new_world_pos = grid_control.odd_worldspace_center()[j, i];
                        new_grid_coord = new Vector2Int(j, grid_control.grid_dimensions.y - (i + 1));
                    }
                }
            }

            //Set the current selection's position to the new world position.
            current_selection.transform.position = new_world_pos;

            //To place a machine down, send the new grid coordinate (index) to the method.
            //Only called when menu buttonclick is false to not conflict with resetting selection.
            if (MachineSelectMenu.buttonClicked == false) 
            {
                set_machine(new_grid_coord);
            }
        }
    }

    //Method to set a machine down on a grid when left mouse click is RELEASED. Takes in the new grid coordinate (index) value.
    //Registers the machine in the abstract grid array.
    void set_machine(Vector2Int new_coord)
    {
        if (Input.GetMouseButtonUp(0))
        {
            grid_control.add_machine(new_coord, current_selection);
            Destroy(current_selection);
        }
    }

}
