using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Items : MonoBehaviour
{
    public Sprite sprite;
    public int ItemIndex;
    public Items[] Madeof;
    [Tooltip("This is for stacking mutiple same items together,if an item is not to be stacked, simply set it to 1")]
    public int MaxStack;
}
