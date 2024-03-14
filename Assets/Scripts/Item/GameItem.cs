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

    SpriteRenderer sr;

    private void Start()
    {
        
    }
}
