using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    public SOItem soitem;

    public string ItemName;
    public Sprite Sprite;
    public List<SOItem> MadeOf = new List<SOItem>();
    //[ColorfulIntAttribute]
    public int Tier;

    public int Count = 1;

    SpriteRenderer sr;

    private void Start()
    {
        
    }

    public void Initialize()
    {
        sr = GetComponent<SpriteRenderer>();
        if (soitem != null) 
        {
            ItemName = soitem.ItemName;
            gameObject.name = soitem.ItemName;
            Sprite = soitem.Sprite;
            if (Sprite != null)
            {
                sr.sprite = Sprite;
            }
            MadeOf = soitem.MadeOf;
            Tier = soitem.Tier;
            Debug.Log($"{soitem.ItemName} initialized!");
        }
        else
        {
            Debug.Log("Item Refrence is not SOItem");
        }
    }
}
