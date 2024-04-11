using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MachinePlacer : MonoBehaviour
{
    public GameObject removalScreen;
    GameObject removeScreen;
    public PlayerResources resources;

    public TMP_Text NotEnoughEnergy;

    //Gameobject set to the currently selected machine. Follows the mouse until set on the grid.
    GameObject current_selection;

    GridController grid_control;

    Vector2 mousePos;

    //World space position on the grid for the selected machine to snap to.
    Vector2 new_world_pos;

    Vector2Int center_pos_index;
    //Index of the abstract grid to set the machine to when placed.
    Vector2Int new_grid_coord;

    public AudioSource placementSFX;

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

    bool deleteState;
    Vector2Int lastMachineCoord;
    GameObject lastMachine;

    void Start()
    {
        grid_control = GetComponent<GridController>();
        levelPlacer();
    }
   
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MachineRemover();
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

            if (MachineSelectMenu.selectionName == "Constuctor")
            {
                Destroy(current_selection);
                current_selection = Instantiate(constructor_prefab, mousePos, Quaternion.identity);
            }

            //
            if (MachineSelectMenu.selectionName == "DeleteButton")
            {
                deleteState = true;
                current_selection = null;
            }
        }
    }

    public void levelPlacer()
    {
        Vector2Int grid_coord;
        print(GridController.levelMap[8,12]);
        int row = 9;
        int col = 13;
        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                if (GridController.levelMap[i,j] == 0)
                {
                    continue;
                }
                else if (GridController.levelMap[i,j] == 1)
                {
                    grid_coord = new Vector2Int(j, row - (i + 1));
                    grid_control.add_machine(grid_coord, collector_prefab);
                }
                else if (GridController.levelMap[i,j] == 2)
                {
                    grid_coord = new Vector2Int(j, row - (i + 1));
                    grid_control.add_machine(grid_coord, storage_prefab);
                }
                else if (GridController.levelMap[i, j] == 3)
                {
                    grid_coord = new Vector2Int(j, row - (i + 1));
                    grid_control.add_machine(grid_coord, transporter_up_prefab);
                }
                //Add more machines to be placed on start
                /*else if (GridController.levelMap[i, j] == 4)
                {
                    grid_coord = new Vector2Int(GridController.gridDim.y - i, j - 1);
                    grid_control.add_machine(grid_coord, //DESIRED PREFAB HERE);
                }*/

            }
        }   
    }

    //Method for snapping a selected, hovering machine to the grid as it moves with the mouse position.
    //This method calls the set_machine() method.
    void machine_placer()
    {
        //The distance between the mouse position and the currently closest cell position.
        //Updates to the shortest distance through iterative checking of cell positions.  
        float closestDistance = 0;

        //If a machine is currently selected,
        if (current_selection != null)
        {
            //Clear Selection on Right click while holding selection
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(current_selection.gameObject);
                return;
            }


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
                current_selection.transform.position = GridController.entire_odd_center_pos[new_grid_coord.x, GridController.gridDim.y - (new_grid_coord.y + 1)];
            }

            //If cell unoccupied, simply set calculated new world pos as position.
            else
            {
                //Set the current selection's position to the new world position.
                current_selection.transform.position = new_world_pos;
            }

            print(center_pos_index);
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
            if(resources.Energy >= current_selection.GetComponent<Machine>().energyCost)
            {
                grid_control.add_machine(new_coord, current_selection);
                resources.Energy -= current_selection.GetComponent<Machine>().energyCost;
                placementSFX.Play();
                Destroy(current_selection);
            }
            else
            {
                Instantiate(NotEnoughEnergy, current_selection.transform.position, Quaternion.identity);
                Destroy(current_selection);
            }
            var data = new machinePlacementData()
            {
                name = current_selection.name,
                location = new_coord,
                cost = 0
            };

            TelemetryLogger.Log(this, "MachinePlaced", data);
        }
    }

    [System.Serializable]
    public struct machinePlacementData
    {
        public string name;
        public Vector2Int location;
        public int cost;
    }

    void MachineRemover()
    {
        if (deleteState)
        {
            if(removeScreen == null)
            {
                Vector3 pos = new Vector3(0, 500, 0);
                removeScreen = Instantiate(removalScreen, MachineSelectMenu.uiObject.transform);
            }
            removeScreen.transform.localScale = new Vector3(Mathf.Sin(Time.deltaTime) * 20 + 1, Mathf.Sin(Time.deltaTime) * 10 + 1, 1);

            new_world_pos = new Vector2(Mathf.Clamp(Mathf.Floor(mousePos.x + 0.5f), -GridController.gridDim.x / 2, GridController.gridDim.x / 2),
                Mathf.Clamp(Mathf.Floor(mousePos.y + 0.5f), -GridController.gridDim.y / 2, GridController.gridDim.y / 2));

            //Matches current calculated new position with that same position stored in the odd center position list.
            int listIndex = GridController.oddCenterPosList.IndexOf(new_world_pos);

            //Calculation of converting the list index to the 2D array index. Matches the index of entire_odd_center_pos
            center_pos_index = new Vector2Int(listIndex % grid_control.grid_dimensions.x, ((listIndex - (listIndex % grid_control.grid_dimensions.x)) / grid_control.grid_dimensions.x));

            //Calculation of converting entire_odd_center_pos' index to gameObject grid index.
            new_grid_coord = new Vector2Int(center_pos_index.x, GridController.gridDim.y - (center_pos_index.y + 1));


            if (lastMachine != null && GridController.grid[new_grid_coord.x, new_grid_coord.y] != lastMachine)
            {
                lastMachine.GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (GridController.grid[new_grid_coord.x, new_grid_coord.y] != null)
            {
                lastMachineCoord = new Vector2Int(new_grid_coord.x, new_grid_coord.y);
                lastMachine = GridController.grid[new_grid_coord.x, new_grid_coord.y].gameObject;
                lastMachine.GetComponent<SpriteRenderer>().color = Color.red;
                if (Input.GetMouseButtonDown(0))
                {
                    grid_control.remove_machine(new_grid_coord, GridController.grid[new_grid_coord.x, new_grid_coord.y].gameObject);
                    GameObject.Destroy(GridController.grid[new_grid_coord.x, new_grid_coord.y]);
                }
            }

            if(Input.GetKeyDown(KeyCode.N))
            {
                GameObject.Destroy(removeScreen);
                lastMachine.GetComponent<SpriteRenderer>().color = Color.white;
                deleteState = false;
            }
        }
    }
}

