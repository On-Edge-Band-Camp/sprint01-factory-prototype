using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour 
{

    //DEPRICATED
    protected Vector2[,] cell_center_pos;
    protected Vector2[,] line_center_pos;

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
        add_machine(new Vector2Int(8,8), storage_prefab);


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
    protected void add_machine(Vector2Int coord, GameObject machine_prefab)
    {

        GameObject new_machine = Instantiate(machine_prefab);

        new_machine.GetComponent<Machine>().grid_coord = coord;

        Vector2 nonIntCoord = new Vector2(coord.x, coord.y);

        grid[coord.x, coord.y] = new_machine;

        new_machine.transform.position = new Vector3(cell_dimensions.x * coord.x - Mathf.FloorToInt(grid_dimensions.x / 2), cell_dimensions.y * coord.y - Mathf.FloorToInt(grid_dimensions.y / 2));

        machines[new_machine.GetComponent<Machine>().machine_type].Add(coord);


        //DEPRICATED CODE

        //center_positions();
        //for(int i = 0; i < grid_dimensions.y; i++)
        //{
        //    for(int j = 0; j < grid_dimensions.x; j++)
        //    {
        //        if (cell_center_pos[j,i] == nonIntCoord)
        //        {
        //            grid[j,i] = new_machine;
        //            machine_array_index = new Vector2Int(j,i);
        //            new_machine.transform.position = new Vector3(cell_center_pos[j,i].x, cell_center_pos[j, i].y);
        //        }
        //    }
        //}
    }

    //Remove a machine from the grid using coordinate
    void remove_machine(Vector2Int coord)
    {

    }

    //Remove a machine from the grid using GameObject
    void remove_machine(GameObject machine)
    {

    }


    //DEPRICATED

    //public void center_positions()
    //{
    //    cell_center_pos = new Vector2[grid_dimensions.x, grid_dimensions.y];
    //    line_center_pos = new Vector2[grid_dimensions.x - 1, grid_dimensions.y - 1];

    //    cell_center_top_left = new Vector2(-(grid_dimensions.x * cell_dimensions.x - cell_dimensions.x) / 2,
    //        (grid_dimensions.y * cell_dimensions.y - cell_dimensions.y) / 2);
    //    line_center_top_left = new Vector2(-(grid_dimensions.x - 1 * cell_dimensions.x - cell_dimensions.x) / 2,
    //        (grid_dimensions.y - 1 * cell_dimensions.y - cell_dimensions.y) / 2);

    //    //Calculating center positions for both cell and line centers
    //    for (int i = 0; i < grid_dimensions.y; i++)
    //    {
    //        for (int j = 0; j < grid_dimensions.x; j++)
    //        {
    //            cell_center_pos[j, i] = new Vector2(cell_center_top_left.x + cell_dimensions.x * j, cell_center_top_left.y - cell_dimensions.y * i);
    //        }
    //    }
    //    for (int i = 0; i < grid_dimensions.y - 1; i++)
    //    {
    //        for (int j = 0; j < grid_dimensions.x - 1; j++)
    //        {
    //            line_center_pos[j, i] = new Vector2(line_center_top_left.x + cell_dimensions.x * j, line_center_top_left.y - cell_dimensions.y * i);
    //        }
    //    }
    //}

}
