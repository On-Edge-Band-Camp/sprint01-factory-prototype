using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryItem : MonoBehaviour
{
    public TMP_Text countText;

    public Items item;
    public int count = 1;
    public Image image;

    public void InitializeItem(Items newItem)
    {
        item = newItem;
        image.sprite = newItem.sprite;
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
    }

    public void DestroySelf()
    {
        Debug.Log("Self-Destruct Started");
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
