using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePlacer : GridController
{
    GameObject current_selection;

    Vector2 mousePos;
    Vector2 newMachinePos;

    float distance;
    float closestDistance;
    void Start()
    {
        //base.center_positions();
    }


    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MachineSelect();
        for(int i = 0; i < grid_dimensions.y; i++)
        {
            for(int j = 0; j < grid_dimensions.x; j++)
            {
                if (grid[j,i] != null)
                {
                    continue;
                }
                distance = Vector2.Distance(mousePos, cell_center_pos[j, i]);
                if(i == 0 && j == 0)
                {
                    closestDistance = distance;
                    newMachinePos = cell_center_pos[j, i];
                }
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    newMachinePos = cell_center_pos[j, i];
                }
            }
        }
        if (current_selection != null)
        {
            current_selection.transform.position = newMachinePos;

            if(Input.GetMouseButtonDown(0))
            {
                Vector2Int tempCoord = new Vector2Int((int)newMachinePos.x, (int)newMachinePos.y);
                add_machine(tempCoord, current_selection);
                Destroy(current_selection);
            }
        }

    }


    void MachineSelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            current_selection = Instantiate(collector_prefab, mousePos, Quaternion.identity);
        }
    }

}
