using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayMachineUI : MonoBehaviour
{
    GameObject UICanvas;
    public GameObject UIToUse;
    GameObject MachineUI;

    Machine Machine;

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MachineUI.transform.localScale = new Vector3(1, 1, 1);
        }
    }


    void Start()
    {
        UICanvas = GameObject.FindWithTag("UICanvas");

        MachineUI = Instantiate(UIToUse, UICanvas.transform);
        MachineUI.transform.localScale = new Vector3(0, 0, 0);

        Machine = GetComponent<Machine>();
        Machine.MachineUI = MachineUI;
        Machine.Inventory = MachineUI.GetComponentInChildren<InventoryManager>();

        TMP_Text MachineName = MachineUI.transform.Find("Title").GetComponent<TMP_Text>();
        MachineName.text = Machine.gameObject.name.ToString();
    }

    void Update()
    {
        
    }
}
