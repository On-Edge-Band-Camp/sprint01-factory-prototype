using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MachinePlacer : MonoBehaviour
{
    //Gameobject set to the currently selected machine. Follows the mouse until set on the grid.
    GameObject current_selection;

    GridController grid_control;

    Vector2 mousePos;

    //World space position on the grid for the selected machine to snap to.
    Vector2 new_world_pos;

    Vector2Int center_pos_index;
    //Index of the abstract grid to set the machine to when placed.
    Vector2Int new_grid_coord;

    //For 2x2's.
    Vector2Int world_pos_index;
    List<Vector2Int> world_pos_indeces = new List<Vector2Int>();
    List<Vector2Int> new_grid_coords = new List<Vector2Int>();
    public static Vector2 evenPlacementPos;
    Vector2 prevPos;

    //String to check if machine's dimensions are odd or even
    public static string machineDimension;

    //All machine prefabs
    public GameObject collector_prefab;
    public GameObject transporter_left_prefab;
    public GameObject transporter_right_prefab;
    public GameObject transporter_up_prefab;
    public GameObject transporter_down_prefab;
    public GameObject storage_prefab;
    public GameObject splitter_prefab;
    public GameObject constructor_prefab;

    public Vector2 worldposcheck;
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
                machineDimension = "Odd";
                current_selection = Instantiate(collector_prefab, mousePos, Quaternion.identity);
            }

            //Transporter Selection
            if (MachineSelectMenu.selectionName == "TransporterLeft")
            {
                Destroy(current_selection);
                machineDimension = "Odd";
                current_selection = Instantiate(transporter_left_prefab, mousePos, Quaternion.identity);
            }

            if (MachineSelectMenu.selectionName == "TransporterRight")
            {
                Destroy(current_selection);
                machineDimension = "Odd";
                current_selection = Instantiate(transporter_right_prefab, mousePos, Quaternion.identity);
            }

            if (MachineSelectMenu.selectionName == "TransporterUp")
            {
                Destroy(current_selection);
                machineDimension = "Odd";
                current_selection = Instantiate(transporter_up_prefab, mousePos, Quaternion.identity);
            }

            if (MachineSelectMenu.selectionName == "TransporterDown")
            {
                Destroy(current_selection);
                machineDimension = "Odd";
                current_selection = Instantiate(transporter_down_prefab, mousePos, Quaternion.identity);
            }

            //Storage Selection
            if (MachineSelectMenu.selectionName == "Storage")
            {
                Destroy(current_selection);
                machineDimension = "Odd";
                current_selection = Instantiate(storage_prefab, mousePos, Quaternion.identity);
            }

            //Splitter Selection
            if (MachineSelectMenu.selectionName == "Splitter")
            {
                Destroy(current_selection);
                machineDimension = "Odd";
                current_selection = Instantiate(splitter_prefab, mousePos, Quaternion.identity);
            }

            if (MachineSelectMenu.selectionName == "Constuctor")
            {
                Destroy(current_selection);
                machineDimension = "Even";
                current_selection = Instantiate(constructor_prefab, mousePos, Quaternion.identity);
            }
        }
    }

    //Method for snapping a selected, hovering machine to the grid as it moves with the mouse position.
    //This method calls the set_machine() method.
    void machine_placer()
    {

        //If a machine is currently selected,
        if (current_selection != null)
        {
            if(machineDimension == "Odd")
            {
                odd_placer();
            }
            else if (machineDimension == "Even")
            {
                even_placer();
            }
            print(new_grid_coords.Count);
            //To place a machine down, send the new grid coordinate (index) to the method.
            //Only called when menu buttonclick is false to not conflict with resetting selection.
            if (MachineSelectMenu.buttonClicked == false) 
            {
                if (machineDimension == "Odd")
                {
                    set_machine(new_grid_coords);
                }
                else
                {
                    set_machine(new_grid_coords);
                }
            }
        }
    }

    //Method to set a machine down on a grid when left mouse click is RELEASED. Takes in the new grid coordinate (index) value.
    //Registers the machine in the abstract grid array.
    void set_machine(List<Vector2Int> new_coord)
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (machineDimension == "Odd")
            {
                grid_control.add_machine(new_coord, current_selection);
                Destroy(current_selection);
            } 
            else
            {
                for(int i = 0; i < 4; i++)
                {
                    grid_control.add_machine(new_coord, current_selection);
                }
                Destroy(current_selection);
            }
        }
    }

    void odd_placer()
    {
        //The distance between the mouse position and the currently closest cell position.
        //Updates to the shortest distance through iterative checking of cell positions.  
        float closestDistance = 0;

        //Get new position for selected machine by simply flooring mousePos + 0.5 (half cell size). Would probably change to a varible when cells
        //bigger than 1x1 are being implemented.
        //Constrains within grid dimension bounds by using Clamp.
        new_world_pos = new Vector2(Mathf.Clamp(Mathf.Floor(mousePos.x + 0.5f), -GridController.gridDim.x / 2, GridController.gridDim.x / 2),
            Mathf.Clamp(Mathf.Floor(mousePos.y + 0.5f), -GridController.gridDim.y / 2, GridController.gridDim.y / 2));

        //Matches current calculated new position with that same position stored in the odd center position list.
        int listIndex = GridController.oddCenterPosList.IndexOf(new_world_pos);

        //Calculation of converting the list index to the 2D array index. Matches the index of entire_odd_center_pos
        center_pos_index = new Vector2Int(listIndex % grid_control.grid_dimensions.x, ((listIndex - (listIndex % grid_control.grid_dimensions.x)) / grid_control.grid_dimensions.x));

        //Calculation of converting entire_odd_center_pos' index to gameObject grid index.
        new_grid_coord = new Vector2Int(center_pos_index.x, GridController.gridDim.y - (center_pos_index.y + 1));


        //If a machine already exists at the current potential placement position,
        if (GridController.grid[new_grid_coord.x, new_grid_coord.y] != null)
        {
            //To be used as calculated index of a neighbouring cell in 3x3 grid around the current potential position.
            Vector2Int checkingIndex;

            //Nested loop going over the 3x3 grid surrounding the current potential position that is already occupied.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //1 is for cell size. Would probably work with a variable of other cell sizes. Check later.
                    int xIncrement = -1;
                    int yIncrement = -1;

                    //Skip iteration if both i,j = 0, since it changes nothing.
                    if (i == 0 && j == 0)
                        continue;

                    //Loop calculations doing combinations of +x, -x, +y, -y, +0 to index.
                    //Set current calculated position as checkingIndex.
                    if (i == 0)
                        xIncrement *= i;
                    else
                        xIncrement = (int)Mathf.Pow(-1, i);
                    if (j == 0)
                        yIncrement *= j;
                    else
                        yIncrement = (int)Mathf.Pow(-1, j);

                    checkingIndex = new Vector2Int(Mathf.Clamp(center_pos_index.x + xIncrement, 0, GridController.gridDim.x - 1),
                        Mathf.Clamp(center_pos_index.y + yIncrement, 0, GridController.gridDim.y - 1));

                    //If that index is occupied, skip iteration and keep checking.
                    if (GridController.grid[checkingIndex.x, GridController.gridDim.y - (checkingIndex.y + 1)] != null)
                        continue;

                    //Once unoccupied cell is found, get distance between mousePos and that cell.
                    float distance = Vector2.Distance(mousePos, GridController.entire_odd_center_pos[checkingIndex.x, checkingIndex.y]);

                    //If current closestDist is 0 or above distance is less than current closestDist, set closestDist to distance and 
                    //center_pos_index to the new index of checkingIndex.
                    if (closestDistance == 0 || distance < closestDistance)
                    {
                        closestDistance = distance;
                        center_pos_index = checkingIndex;
                    }


                }
            }
            new_world_pos = GridController.entire_odd_center_pos[center_pos_index.x, center_pos_index.y];
            new_grid_coord = new Vector2Int(center_pos_index.x, GridController.gridDim.y - (center_pos_index.y + 1));
        }
        //If cell unoccupied, simply set calculated new world pos as position.
        else
        {
            //Set the current selection's position to the new world position.
            current_selection.transform.position = new_world_pos;
        }
        if (new_grid_coords.Count > 0)
            new_grid_coords.RemoveRange(0, 1);
        new_grid_coords.Add(new_grid_coord);
    }

    void even_placer()
    {
        //The distance between the mouse position and the currently closest cell position.
        //Updates to the shortest distance through iterative checking of cell positions.  
        float closestDistance = 0;
        prevPos = current_selection.transform.position;
        //Get new position for selected machine by simply flooring mousePos + 0.5 (half cell size). Would probably change to a varible when cells
        //bigger than 1x1 are being implemented.
        //Constrains within grid dimension bounds by using Clamp.
        new_world_pos = new Vector2(Mathf.Clamp(Mathf.Floor(mousePos.x + 0.50f), -GridController.gridDim.x / 2 + 1, GridController.gridDim.x / 2 - 1),
            Mathf.Clamp(Mathf.Floor(mousePos.y + 0.50f), -GridController.gridDim.y / 2 + 1, GridController.gridDim.y / 2 - 1));

        //Position correction from cell center to line intersection depending on mouse's position relative to currently selected cell center.
        if (mousePos.x > new_world_pos.x)
            new_world_pos.x += 0.5f;
        else if(mousePos.x < new_world_pos.x)
            new_world_pos.x -= 0.5f;
        if (mousePos.y > new_world_pos.y)
            new_world_pos.y += 0.5f;
        else if (mousePos.y < new_world_pos.y)
            new_world_pos.y -= 0.5f;

        //Matches current calculated new position with that same position stored in the even center position list.
        int listIndex = GridController.evenCenterPosList.IndexOf(new_world_pos);

        //Calculation of converting the list index to the 2D array index. Matches the index of entire_even_center_pos
        center_pos_index = new Vector2Int(listIndex % (grid_control.grid_dimensions.x - 1), ((listIndex - (listIndex % (grid_control.grid_dimensions.x - 1))) / (grid_control.grid_dimensions.x - 1)));
        evenPlacementPos = GridController.entire_even_center_pos[center_pos_index.x, center_pos_index.y];
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                if (world_pos_indeces.Count > 4)
                {
                    world_pos_indeces.RemoveRange(0, 4);
                }
                if(new_grid_coords.Count > 4)
                {
                    new_grid_coords.RemoveRange(0, 4);
                }
                world_pos_index = new Vector2Int(center_pos_index.x + i, center_pos_index.y + j);
                world_pos_indeces.Add(world_pos_index);
                //Calculation of converting entire_even_center_pos' index to gameObject grid index.
                new_grid_coord = new Vector2Int(world_pos_index.x, GridController.gridDim.y - (world_pos_index.y + 1));
                new_grid_coords.Add(new_grid_coord);
            }
        }
        //print(new_grid_coords[1]);
        Vector2Int checkingIndex;
        List<Vector2Int> checkingIndeces = new List<Vector2Int>();
        for (int i = 0; i < 4; i++) 
        {
            //If a machine already exists at the current potential placement position,
            if (GridController.grid[new_grid_coords[i].x, new_grid_coords[i].y] != null)
            {
                print(prevPos);

                current_selection.transform.position = prevPos;
                
                /*///To be used as calculated index of a neighbouring cell in 3x3 grid around the current potential position.

                 //Nested loop going over the 3x3 grid surrounding the current potential position that is already occupied.
                 for (int j = 0; j < 3; j++)
                 {
                     for (int k = 0; k < 3; k++)
                     {
                         //1 is for cell size. Would probably work with a variable of other cell sizes. Check later.
                         int xIncrement = -1;
                         int yIncrement = -1;

                         //Skip iteration if both i,j = 0, since it changes nothing.
                         if (j == 0 && k == 0)
                             continue;

                         //Loop calculations doing combinations of +x, -x, +y, -y, +0 to index.
                         //Set current calculated position as checkingIndex.
                         if (j == 0)
                             xIncrement *= j;
                         else
                             xIncrement = (int)Mathf.Pow(-1, j);
                         if (k == 0)
                             yIncrement *= k;
                         else
                             yIncrement = (int)Mathf.Pow(-1, k);

                         checkingIndex = new Vector2Int(Mathf.Clamp(world_pos_indeces[i].x + xIncrement, 0, GridController.gridDim.x - 1),
                             Mathf.Clamp(world_pos_indeces[i].y + yIncrement, 0, GridController.gridDim.y - 1));
                         checkingIndeces.Add(checkingIndex);

                         for (int l = 0; l < 4; l++)
                         {
                             //If that index is occupied, skip iteration and keep checking.
                             if (GridController.grid[checkingIndeces[l].x, GridController.gridDim.y - (checkingIndeces[l].y + 1)] != null)
                                 continue;
                         }

                         //Once unoccupied cell is found, get distance between mousePos and that cell.
                         float distance = Vector2.Distance(mousePos, GridController.entire_even_center_pos[checkingIndex.x, checkingIndex.y]);

                         //If current closestDist is 0 or above distance is less than current closestDist, set closestDist to distance and 
                         //center_pos_index to the new index of checkingIndex.
                         if (closestDistance == 0 || distance < closestDistance)
                         {
                             closestDistance = distance;
                             center_pos_index = checkingIndex;
                         }


                     }
                 }
                 new_world_pos = GridController.entire_even_center_pos[center_pos_index.x, center_pos_index.y];*/
            }
            //If cell unoccupied, simply set calculated new world pos as position.
            else
            {
                //Set the current selection's position to the new world position.
                current_selection.transform.position = new_world_pos;
            }
        }
    }

}
