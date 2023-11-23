using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour 
{

    //Grid which stores gameobject references of any machines placed within it
    public static GameObject[,] grid;

    //Initializer dimensions of the grid
    public Vector2Int grid_dimensions = new Vector2Int(8, 8);

    //How large cells appear in the scene
    public Vector2 cell_dimensions = new Vector2(1,1);

    //DEPRICATED CODE
    //The center of the grid in the scene
    //public Vector2 grid_origin = new Vector2();

    //List dictionary for each individual type of machines coordinates, sorted by machine type
    Dictionary<string, List<Vector2Int>> machines = new Dictionary<string, List<Vector2Int>>()
    {
        {"collector", new List<Vector2Int>()},
        {"transporter", new List<Vector2Int>()},
        {"combiner", new List<Vector2Int>()},
        {"storage", new List<Vector2Int>()},
    };


    //All machine prefabs

    public GameObject collector_prefab;
    public GameObject transporter_prefab;
    public GameObject storage_prefab;

    private void Start()
    {
        init_grid();
    }

    private void Update()
    {
        update_machines(new string[] {"collector","transporter"});
    }

    //Initializes the grid on startup
    //THIS VERSION OF THIS METHOD IS FOR TESTING ONLY, NOT COMPLETE
    void init_grid()
    {

        grid = new GameObject[grid_dimensions.x, grid_dimensions.y];

        add_machine(new Vector2Int(), storage_prefab);
        add_machine(new Vector2Int(1, 1), collector_prefab);
        add_machine(new Vector2Int(2, 1), transporter_prefab);
        add_machine(new Vector2Int(3, 1), transporter_prefab);
        add_machine(new Vector2Int(4, 1), transporter_prefab);
        add_machine(new Vector2Int(5, 1), storage_prefab);
        add_machine(new Vector2Int(8, 8), storage_prefab);


    }

    //Sends update calls to all machines in the grid according to the update_order, which is an array of machine type strings
    //THIS VERSION OF THIS METHOD IS FOR TESTING ONLY, NOT COMPLETE
    void update_machines(string[] update_order)
    {

        foreach(string machine_type in update_order)
        {
            foreach(Vector2Int coordinate in machines[machine_type])
            {
                grid[coordinate.x, coordinate.y].GetComponent<Machine>().update_machine();

                //DEPRICATED CODE
                //grid[machine_array_index.x, machine_array_index.y].GetComponent<Machine>().update_machine();
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
    void remove_machine(Vector2Int coord)
    {

    }

    //Remove a machine from the grid using GameObject
    void remove_machine(GameObject machine)
    {

    }

    //Returns a 2D Vector2 array of center positions for individual grid cell in worldspace.
        //To be used by ODD-GRID machines.
    public Vector2[,] odd_worldspace_center()
    {

        //The return variable. An array of world space center positions for odd-grid machines (center of grid cell). Identical in size to base grid array.
        Vector2[,] odd_center_pos = new Vector2[grid_dimensions.x, grid_dimensions.y];


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
                odd_center_pos[j, i] = new Vector2(odd_center_top_left.x + cell_dimensions.x * j, odd_center_top_left.y - cell_dimensions.y * i);
            }
        }

        return odd_center_pos;
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
