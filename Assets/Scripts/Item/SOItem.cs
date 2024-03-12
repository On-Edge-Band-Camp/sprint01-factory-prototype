using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionImporter.Asset", menuName = "Items/New Item")]
public class SOItem : ScriptableObject
{
    public string ItemName;
    public Sprite Sprite;
    public List <SOItem> MadeOf = new List<SOItem>();
    [ColorfulIntAttribute]
    public int Tier;
}
