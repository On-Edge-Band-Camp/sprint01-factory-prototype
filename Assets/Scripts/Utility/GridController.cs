using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum machine_types
{

    Collector = 101,
    Transporter = 102,
    Constructor = 103,
    Deconstructor = 104,
    Storage = 105,
    Splitter = 106,
    Combiner = 107

};


public class GridController : MonoBehaviour 
{
    //Grid which stores gameobject references of any machines placed within it
    public static GameObject[,] grid;

    //Deprecated, check and remove later.
    //Center positions restricted to camera size.
    //public Vector2[,] camera_odd_center_pos;

    //2D array mirroring the game grid that stores the world positions of every cell-centers in the grid.
    public static Vector2[,] entire_odd_center_pos;
    //2D array mirroring the game grid that stores the world positions of every line-intersections in the grid.
    public static Vector2[,] entire_even_center_pos;
    //List containing each cell/line position (starting from top left, moving right first and downwards)
        //Used to match possible machine placement position in machinePlacer by using .IndexOf().
    public static List<Vector2> oddCenterPosList = new List<Vector2>();
    public static List<Vector2> evenCenterPosList = new List<Vector2>();

    //Initializer dimensions of the grid
    public Vector2Int grid_dimensions = new Vector2Int(21, 25);
    //Static grid dimension variable for use in machinePlacer.
    public static Vector2Int gridDim;

    //TEMPORARY dimensions of grid visible within camera
    //Will be changed later to accomodate zoom/size change
    public Vector2Int camera_grid = new Vector2Int(21, 11);

    //How large cells appear in the scene
    public Vector2 cell_dimensions = new Vector2(1,1);

    //List dictionary for each individual type of machines coordinates, sorted by machine type
    //see machine class for machine type id reference
    Dictionary<machine_types, List<Vector2Int>> machines = new Dictionary<machine_types, List<Vector2Int>>()
    {
        {machine_types.Collector, new List<Vector2Int>()},
        {machine_types.Transporter, new List<Vector2Int>()},
        {machine_types.Constructor, new List<Vector2Int>()},
        {machine_types.Deconstructor, new List<Vector2Int>()},
        {machine_types.Storage, new List<Vector2Int>()},
        {machine_types.Splitter, new List<Vector2Int>()},
        {machine_types.Combiner, new List<Vector2Int>()}
    };

    //Initialize Grid
    private void Start()
    {

        gridDim = grid_dimensions;
        init_grid();
        odd_worldspace_center();
        even_worldspace_center();
    }

    //Runs each frame
    private void Update()
    {
        machine_types[] update_order = {
            
            machine_types.Transporter,
            machine_types.Splitter,
            machine_types.Collector,
            machine_types.Constructor
        
        };

        update_machines(update_order);

    }

    //Initializes the grid on startup
    //THIS VERSION OF THIS METHOD IS FOR TESTING ONLY, NOT COMPLETE
    void init_grid()
    {

        grid = new GameObject[grid_dimensions.x, grid_dimensions.y];

    }

    //Sends update calls to all machines in the grid according to the update_order, which is an array of machine type strings
    //THIS VERSION OF THIS METHOD IS FOR TESTING ONLY, NOT COMPLETE
    void update_machines(machine_types[] update_order)
    {

        foreach(machine_types machine_type in update_order)
        {

            foreach(Vector2Int coordinate in machines[machine_type])
            {
                grid[coordinate.x, coordinate.y].GetComponent<Machine>().update_machine();
            }
        }

    }

    //Add a new machine to the grid
    //THIS VERSION OF THIS METHOD IS FOR TESTING ONLY, NOT COMPLETE
    public void add_machine(List<Vector2Int> coord, GameObject machine_prefab)
    {
        //List<Vector2Int> machineCoord = coord;
        Vector2 nonIntCoord;
        GameObject new_machine = machine_prefab;
        if (MachinePlacer.machineDimension == "Odd")
        {
            new_machine = Instantiate(machine_prefab);
            new_machine.GetComponent<Machine>().grid_coord = coord[0];
            nonIntCoord = new Vector2(coord[0].x, coord[0].y);

            grid[coord[0].x, coord[0].y] = new_machine;

            new_machine.transform.position = new Vector3(cell_dimensions.x * coord[0].x - Mathf.FloorToInt(grid_dimensions.x / 2),
                cell_dimensions.y * coord[0].y - Mathf.FloorToInt(grid_dimensions.y / 2));

            machines[new_machine.GetComponent<Machine>().machine_type].Add(coord[0]);
        }
        if(MachinePlacer.machineDimension == "Even")
        {
 
            new_machine = Instantiate(machine_prefab, MachinePlacer.evenPlacementPos, Quaternion.identity);
            for (int i = 0; i < 4; i++)  
            {
                //new_machine = Instantiate(machine_prefab, entire_odd_center_pos[coord[i].x, Mathf.Abs(coord[i].y - (gridDim.y - 1))], Quaternion.identity);
                new_machine.GetComponent<Machine>().grid_coord = coord[i];
                nonIntCoord = new Vector2(coord[i].x, coord[i].y);

                grid[coord[i].x, coord[i].y] = new_machine;

                Vector3 tester = new Vector3(cell_dimensions.x * coord[i].x - Mathf.FloorToInt(grid_dimensions.x / 2),
                    cell_dimensions.y * coord[i].y - Mathf.FloorToInt(grid_dimensions.y / 2));

                machines[new_machine.GetComponent<Machine>().machine_type].Add(coord[i]);
            }
        }

    }

    //Remove a machine from the grid using coordinate
    void remove_machine(Vector2Int coord)
    {

    }

    //Remove a machine from the grid using GameObject
    void remove_machine(GameObject machine)
    {

    }

    //Returns a 2D Vector2 array of center positions for individual grid cell in worldspace.
        //To be used by ODD-GRID machines.
    public void odd_worldspace_center()
    {

        //The return variable. An array of world space center positions for odd-grid machines (center of grid cell). Identical in size to base grid array.
        entire_odd_center_pos = new Vector2[grid_dimensions.x, grid_dimensions.y];


        //worldspace width and height of the entire grid
        float grid_width = grid_dimensions.x * cell_dimensions.x;
        float grid_height = grid_dimensions.y * cell_dimensions.y;

        //Position of top left corner center position to begin calculations from.
        Vector2 odd_center_top_left = new Vector2(-(grid_width - cell_dimensions.x) / 2, (grid_height - cell_dimensions.y) / 2);


        //Calculating and setting odd center positions for every cell.
            //Begins from top left, increases X by cell width, decreases Y by cell height.
        for (int i = 0; i < grid_dimensions.y; i++)
        {
            for (int j = 0; j < grid_dimensions.x; j++)
            {

                //Iterates through a row first (X is j) and proceeds down on the column (Y is i) through the array.
                entire_odd_center_pos[j, i] = new Vector2(odd_center_top_left.x + cell_dimensions.x * j, odd_center_top_left.y - cell_dimensions.y * i);
                //Adds each center position to a list to be searched with .IndexOf()
                oddCenterPosList.Add(entire_odd_center_pos[j, i]);
            }
        }

    }


    //Returns a 2D Vector2 array of grid line intersections (EXCLUDING CORNERS AND EDGES) in worldspace.
    //To be used by EVEN-GRID machines.
    public void even_worldspace_center()
    {

        //The return variable. An array of world space center positions for even-grid machines (grid line intersection points).
            //Always 1 index less than the base grid array for both x and y due to corners being unoccupiable. 
        entire_even_center_pos = new Vector2[grid_dimensions.x - 1, grid_dimensions.y - 1];


        //worldspace width and height of the REDUCED grid
        float grid_width = grid_dimensions.x - 1 * cell_dimensions.x;
        float grid_height = grid_dimensions.y - 1 * cell_dimensions.y;

        //Position of top left corner center to begin calculations from.
        Vector2 even_center_top_left = new Vector2(-(grid_width - cell_dimensions.x) / 2, (grid_height - cell_dimensions.y) / 2);


        //Calculating and setting odd center positions for every cell.
            //Begins from top left, increases X by cell width, decreases Y by cell height.
        for (int i = 0; i < grid_dimensions.y - 1; i++)
        {
            for (int j = 0; j < grid_dimensions.x - 1; j++)
            {

                //Iterates through a row first (X is j) and proceeds down on the column (Y is i) through the array.
                entire_even_center_pos[j, i] = new Vector2(even_center_top_left.x + cell_dimensions.x * j, even_center_top_left.y - cell_dimensions.y * i);
                //Adds each center position to a list to be searched with .IndexOf()
                evenCenterPosList.Add(entire_even_center_pos[j, i]);
            }
        }

    }

}
