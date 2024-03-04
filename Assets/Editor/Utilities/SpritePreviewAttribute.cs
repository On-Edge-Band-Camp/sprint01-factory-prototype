using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Display a preview image of a sprite in the inspector
/// </summary>
public class SpritePreviewAttribute : PropertyAttribute
{
    public float Height { get; private set; }
    public SpritePreviewAttribute(float hieght = 100)
    {
        Height = hieght;
    }
}
