using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayMachineUI : MonoBehaviour
{
    GameObject UICanvas;
    GameObject MachineUI;

    Machine Machine;

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            MachineUI.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UICanvas = GameObject.FindWithTag("UICanvas");

        GameObject UItoLoad = Resources.Load("UI/MachineUI")as GameObject;
        MachineUI = Instantiate(UItoLoad,UICanvas.transform);
        MachineUI.transform.localScale = new Vector3(0, 0, 0);


        Machine = GetComponent<Machine>();
        Machine.MachineUI = MachineUI;
        Machine.Inventory = MachineUI.GetComponentInChildren<InventoryManager>();

        TMP_Text MachineName = MachineUI.transform.Find("Title").GetComponent<TMP_Text>();
        MachineName.text = Machine.gameObject.name.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
