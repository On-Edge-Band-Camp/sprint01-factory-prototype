using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    public string ItemName;
    public Sprite Sprite;
    public List<GameItem> MadeOf = new List<GameItem>();
    [ColorfulIntAttribute]
    public int Tier;
    [TextArea]
    public string Description;

    SpriteRenderer sr;

    public int ItemCount(GameItem CountingItem)
    {
        int Count = 0;
        foreach(GameItem item in MadeOf)
        {
            if(CountingItem == item)
            {
                Count++;
            }
        }
        return Count;
    }
}
