using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    public string ItemName;
    public Sprite Sprite;
    public List<GameItem> MadeOf = new List<GameItem>();
    //[ColorfulIntAttribute]
    public int Tier;

    SpriteRenderer sr;

    private void Start()
    {
        
    }

    public void Initialize()
    {
        //sr = GetComponent<SpriteRenderer>();
        //if (soitem != null) 
        //{
        //    ItemName = soitem.ItemName;
        //    gameObject.name = soitem.ItemName;
        //    Sprite = soitem.Sprite;
        //    if (Sprite != null)
        //    {
        //        sr.sprite = Sprite;
        //    }
        //    Tier = soitem.Tier;
        //    Debug.Log($"{soitem.ItemName} initialized!");
        //}
        //else
        //{
        //    Debug.Log("Item Refrence is not SOItem");
        //}
    }
}
