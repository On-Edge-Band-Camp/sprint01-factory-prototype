using UnityEngine;
using UnityEditor;

//What class this script is used for
[CustomEditor(typeof(GameItem))]
public class ItemEditor : Editor
{
    GameItem item;
    public override void OnInspectorGUI()
    {
        //Draw the rest of inspector
        base.OnInspectorGUI();

        // Save the originnal UI's color
        Color defalutColor = GUI.color;

        // Inspects the item class? Not exactlly sure what this does.
        // But it is nesseary.
        item = (GameItem)target;

        DrawSpritePreview();

        GUI.color = defalutColor;
    }

    //Create a preview of the sprite
    void DrawSpritePreview()
    {
        if (item.Sprite != null)
        {
            //Get refrence To the sprite image.
            Texture2D sprite = AssetPreview.GetAssetPreview(item.Sprite);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            //Draw the preview of an item's sprite
            GUILayout.BeginVertical();
            GUILayout.Label(sprite, GUILayout.Width(70), GUILayout.Height(70));
            //Create a area which contains the Labeled Text.
            GUILayout.Label(item.ItemName);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
    }
}
