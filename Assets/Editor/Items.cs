using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionImporter.Asset", menuName = "Items/New Item")]
public class Items : ScriptableObject
{
    public string ItemName;
    public Sprite Sprite;
    public List <Items> MadeOf = new List<Items>();
    [ColorfulIntAttribute]
    public int Tier;
}
