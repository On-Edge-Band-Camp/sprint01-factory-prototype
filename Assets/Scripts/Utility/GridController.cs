using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static string winningItem1 = "Water";
    public static string winningItem102 = "Air";

    public static string winningItem2 = "Crystal";

    public static string winningItem3 = "Originisite Dust";
    public static string winningItem302 = "Frostbloom";
    public static string winningItem303 = "Lifefruit";

    public static int winItem1Amount = 5;
    public static int winItem102Amount = 5;

    public static int winItem2Amount = 5;

    public static int winItem3Amount = 23;
    public static int winItem302Amount = 32;
    public static int winItem303Amount = 42;

    public static int currentStage;

    public static bool winState;

    //Visual grid matrix for initial level setup. replace numbers with corresponding int of a machine.
    public static int[,] level0Map = 
        //Currently have Collector = 1, Storage = 2, Constructor = 3, Splitter = 4, Transport Up, Down, Left, Right = 5, 6, 7, 8.
        //Adding other machines happens in levelPlacer() in the MachinePlacer script
        {   {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 2, 7, 4, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}   };
    public static int[,] level1Map =
        {   {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 8, 8, 4, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0},
            {0, 0, 0, 2, 0, 0, 0, 0, 0, 3, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}   };
    public static int[,] level2Map =
       {   {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 1, 8, 8, 8, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 6, 0, 3, 0, 0, 0, 4, 7, 7, 1, 0},
            {0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}   };

    //Deprecated, check and remove later.
    //Center positions restricted to camera size.
    //public Vector2[,] camera_odd_center_pos;

    //2D array mirroring the game grid that stores the world positions of every cell in the grid.
    public static Vector2[,] entire_odd_center_pos;
    //List containing each cell position (starting from top left, moving right first and downwards)
        //Used to match possible machine placement position in machinePlacer by using .IndexOf().
    public static List<Vector2> oddCenterPosList = new List<Vector2>();

    //Initializer dimensions of the grid
    public Vector2Int grid_dimensions = new Vector2Int(13, 9);
    //Static grid dimension variable for use in machinePlacer.
    public static Vector2Int gridDim;

    //TEMPORARY dimensions of grid visible within camera
    //Will be changed later to accomodate zoom/size change
    public Vector2Int camera_grid = new Vector2Int(21, 11);

    //How large cells appear in the scene
    public Vector2 cell_dimensions = new Vector2(1,1);

    public static int numberOfMachines;

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
    private void Awake()
    {
        gridDim = grid_dimensions;
        init_grid();
        odd_worldspace_center();
    }

    //Runs each frame
    private void Update()
    {
        machine_types[] update_order = {

            machine_types.Transporter,
            machine_types.Splitter,
            machine_types.Collector,
            machine_types.Constructor,
            machine_types.Storage
        
        };

        if (numberOfMachines > 0)
        {
            update_machines(update_order);
        }
        print(Storage.winningItemAmount);
        if (SceneManager.GetActiveScene().name == "Level0")
        {
            if (Storage.winningItemAmount >= winItem1Amount && Storage.winningItem2Amount >= winItem102Amount)
            {
                winState = true;
                FindObjectOfType<Machine>().inventory_total = 0;
                for (int i = 0; i < GridController.gridDim.x; i++)
                {
                    for (int j = 0; j < GridController.gridDim.y; j++)
                    {                     
                        GameObject.Destroy(grid[i, j]);
                        numberOfMachines = 0;
                    }
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            if (Storage.winningItemAmount == winItem2Amount)
            {
                winState = true;
                FindObjectOfType<Machine>().inventory_total = 0;
                for (int i = 0; i < GridController.gridDim.x; i++)
                {
                    for (int j = 0; j < GridController.gridDim.y; j++)
                    {
                        GameObject.Destroy(grid[i, j]);
                        numberOfMachines = 0;
                    }
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            if (Storage.winningItemAmount >= winItem3Amount && Storage.winningItem2Amount >= winItem302Amount && Storage.winningItem3Amount >= winItem303Amount)
            {
                winState = true;
                FindObjectOfType<Machine>().inventory_total = 0;
                for (int i = 0; i < GridController.gridDim.x; i++)
                {
                    for (int j = 0; j < GridController.gridDim.y; j++)
                    {
                        GameObject.Destroy(grid[i, j]);
                        numberOfMachines = 0;
                    }
                }
            }
        }
        if (winState)
        {
            currentStage++;
            print(currentStage);
            Storage.winningItemAmount = 0;
            Storage.winningItem2Amount = 0;
            Storage.winningItem3Amount = 0;
            winState = false;
            if(SceneManager.GetActiveScene().name == "Level2")
            {
                SceneMaster.GoToLevel(5);
            }
            else
            {
                SceneMaster.GoToLevel(2);
            }
        }

        //print(grid[6, 6].GetComponent<MachineDetails>().);
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
    public void add_machine(Vector2Int coord, GameObject machine_prefab)
    {

        GameObject new_machine = Instantiate(machine_prefab);

        new_machine.GetComponent<Machine>().grid_coord = coord;

        Vector2 nonIntCoord = new Vector2(coord.x, coord.y);

        grid[coord.x, coord.y] = new_machine;

        new_machine.transform.position = new Vector3(cell_dimensions.x * coord.x - Mathf.FloorToInt(grid_dimensions.x / 2), 
            cell_dimensions.y * coord.y - Mathf.FloorToInt(grid_dimensions.y / 2));

        machines[new_machine.GetComponent<Machine>().machine_type].Add(coord);

    }

    //Remove a machine from the grid using coordinate
    public void remove_machine(Vector2Int coord, GameObject removed_machine)
    {
        machines[removed_machine.GetComponent<Machine>().machine_type].Remove(coord);
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
    public Vector2[,] even_worldspace_center()
    {

        //The return variable. An array of world space center positions for even-grid machines (grid line intersection points).
            //Always 1 index less than the base grid array for both x and y due to corners being unoccupiable. 
        Vector2[,] even_center_pos = new Vector2[grid_dimensions.x - 1, grid_dimensions.y - 1];


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
                even_center_pos[j, i] = new Vector2(even_center_top_left.x + cell_dimensions.x * j, even_center_top_left.y - cell_dimensions.y * i);
            }
        }

        return even_center_pos;
    }

}
