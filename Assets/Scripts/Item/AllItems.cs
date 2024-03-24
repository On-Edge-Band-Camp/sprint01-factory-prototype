using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Item List.Asset", menuName = "Items/Item List")]
public class AllItems : ScriptableObject
{
    public List<GameItem> AllGameItems;
}
