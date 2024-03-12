using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MachineUIDisplay : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject MachineUI;

    bool UIOpened;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("UICanvas");
    }

    private void OnMouseOver()
    {
        Debug.Log("MouseOver");
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!UIOpened)
            {
                Instantiate(MachineUI,Canvas.transform);
                UIOpened = true;
            }          
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
