using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour 
{

    //Grid which stores gameobject references of any machines placed within it
    public static GameObject[,] grid;

    //Initializer dimentions of the grid
    public Vector2Int grid_dimentions;

    //How large cells appear in the scene
    public Vector2 cell_dimentions = new Vector2(1,1);

    //The center of the grid in the scene
    public Vector2 grid_origin = new Vector2();

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

        grid = new GameObject[grid_dimentions.x, grid_dimentions.y];

        add_machine(new Vector2Int(1, 1), collector_prefab);
        add_machine(new Vector2Int(2, 1), transporter_prefab);
        add_machine(new Vector2Int(3, 1), transporter_prefab);
        add_machine(new Vector2Int(4, 1), transporter_prefab);
        add_machine(new Vector2Int(5, 1), storage_prefab);


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
            }
        }

    }

    //Add a new machine to the grid
    //THIS VERSION OF THIS METHOD IS FOR TESTING ONLY, NOT COMPLETE
    void add_machine(Vector2Int coord, GameObject machine_prefab)
    {
        
        GameObject new_machine = Instantiate(machine_prefab);

        new_machine.GetComponent<Machine>().grid_coord = coord;

        grid[coord.x,coord.y] = new_machine;

        new_machine.transform.position = new Vector3(cell_dimentions.x * coord.x, cell_dimentions.y * coord.y);

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

}
