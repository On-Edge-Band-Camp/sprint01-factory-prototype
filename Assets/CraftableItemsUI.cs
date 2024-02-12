using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftableItemsUI : MonoBehaviour
{
    private void Start()
    {
        Close();
    }

    public void Drag()
    {
        transform.position = Input.mousePosition;
    }

    public void Open()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void Close()
    {
        transform.localScale = new Vector3(0,0,0);
    }

    public void AddItemToUI()
    {

    }
}
