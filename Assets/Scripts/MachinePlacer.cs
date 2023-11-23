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

    void Start()
    {
        grid_control = GetComponent<GridController>();
    }


    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        machine_select();
        machine_placer();

    }

    //Basic tester code for ""Selecting"" a machine. Currently hardcoded to spawn a machine when a number key is pressed
    void machine_select()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            current_selection = Instantiate(grid_control.storage_prefab, mousePos, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            current_selection = Instantiate(grid_control.collector_prefab, mousePos, Quaternion.identity);
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
            set_machine(new_grid_coord);
        }
    }

    //Method to set a machine down on a grid with a left mouse click. Takes in the new grid coordinate (index) value.
    //Registers the machine in the abstract grid array.
    void set_machine(Vector2Int new_coord)
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid_control.add_machine(new_coord, current_selection);
            Destroy(current_selection);
        }
    }

}
