using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ColorfulIntAttribute))]
public class ColorfulIntDrawer : PropertyDrawer
{
    //array of possible colors
    static Color32[] TierColors = new[]
    {
        new Color32(252,186,3,255), // T0
        new Color32(245, 81, 100,255), // T1
        new Color32(173, 84, 255,255), // T2
        new Color32(79, 134, 255,255), // T3
        new Color32(94, 230, 121,255), // T4
        new Color32(215, 250, 252,255), // T5
        new Color32(255, 255, 255,255), //Unknown or out of bounds
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(property.propertyType != SerializedPropertyType.Integer)
        {
            Debug.LogError("A Non-Int used the property drawer: ColorfulInts");
            EditorGUI.LabelField(position, label.text, "Please only use ColorfulInt with int.");
            return;
        }

        //The OG color of the inspector.
        Color InitialColor = GUI.color;

        //Change color to the color according to TierColor
        GUI.color = TierColors[Mathf.Clamp(property.intValue, 0, TierColors.Length-1)];
        //Draw the actual field for the value in the inspector
        EditorGUI.PropertyField(position, property, label);
        //Reset the GUI color, so other variables would not be affected.
        GUI.color = InitialColor;
    }
}
