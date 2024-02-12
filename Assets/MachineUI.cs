using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUI : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void close()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }

    public void DragUI()
    {
        transform.position = Input.mousePosition;
    }
}
