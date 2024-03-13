using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FloatingMenu : MonoBehaviour
{
    public GameObject floatingMenuPrefab;
    GameObject menu;
    public RectTransform RectTransform; 

    Vector2 mousePos;

    Vector2 prevMouseGridPos;
    Vector2 halfWidthHeight = new Vector2(1.25f, 1.5f);

    Vector2 mouseGridPos;
    Vector2Int positionGridIndex;
    Vector2Int machineGridIndex;


    //Bool to be set to true the frame the menu is spawned so it doesn't get destroyed with mouse release. Set to false at the end of frame.
    bool spawnInstant;

    void Start()
    {
        
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Exact same method of finding grid position and index relative to mouse position used in machinePlacer. See there for explanation.

        mouseGridPos = new Vector2(Mathf.Clamp(Mathf.Floor(mousePos.x + 0.5f), -GridController.gridDim.x / 2, GridController.gridDim.x / 2),
            Mathf.Clamp(Mathf.Floor(mousePos.y + 0.5f), -GridController.gridDim.y / 2, GridController.gridDim.y / 2));

        int listIndex = GridController.oddCenterPosList.IndexOf(mouseGridPos);

        positionGridIndex = new Vector2Int(listIndex % GridController.gridDim.x, ((listIndex - (listIndex % GridController.gridDim.x)) / GridController.gridDim.x));

        machineGridIndex = new Vector2Int(positionGridIndex.x, GridController.gridDim.y - (positionGridIndex.y + 1));


        //If machine exists at current mouse grid position and mouse is clicked and released, spawn menu.
        if (GridController.grid[machineGridIndex.x, machineGridIndex.y] != null)
        {
            if (Input.GetMouseButtonUp(1))
            {
                if(menu != null)
                {
                    Destroy(menu);
                }
                Machine machine = GridController.grid[machineGridIndex.x, machineGridIndex.y].GetComponent<Machine>();
                menu = Instantiate(floatingMenuPrefab, RectTransform.position, Quaternion.identity, this.transform);
                menu.GetComponent<MachineDetails>().machine = machine;
                menu.GetComponent<MachineDetails>().InputNewInventory(machine);
                machine.UI = menu.GetComponent<MachineDetails>();
                spawnInstant = true;
                prevMouseGridPos = mouseGridPos;                   
            }
        }

        //If mouse released, menu exists, and spawnInstant is false (at least 1 frame has passed since menu spawning)
        if (Input.GetMouseButtonUp(0) && menu != null && !spawnInstant)
        {
            //If this bool is true in menu script, do nothing and set all bools to false.
            //The menu itself is a button and this allows for it to not be destroyed when clicked on (as clicking anywhere outside of a machine's bounds will destroy the menu).
            if (MachineSelectMenu.floatingMenuClicked)
            {
                MachineSelectMenu.buttonClicked = false;
                MachineSelectMenu.floatingMenuClicked = false;
            }
            //If false, destroy menu
            else
            {
                Destroy(menu);
            }
        }
        spawnInstant = false;
    }    
}
