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

    //List dictionary for each individual type of machines coordinates, sorted by machine type
    Dictionary<string, List<Vector2Int>> machines = new Dictionary<string, List<Vector2Int>>()
    {
        {"collector", new List<Vector2Int>()},
        {"transporter", new List<Vector2Int>()},
        {"combiner", new List<Vector2Int>()},
    };

    //Initializes the grid on startup
    void init_grid()
    {

    }

    //Sends update calls to all machines in the grid according to the update_order, which is an array of machine type strings
    void update_machines(string[] update_order)
    {

    }

    //Add a new machine to the grid
    void add_machine(Vector2Int coord, GameObject machine_prefab)
    {

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
