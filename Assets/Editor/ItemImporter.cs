using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ItemImporter.Asset", menuName ="Data/Item Importer")]
public class ItemImporter : ScriptableObject
{
    [Tooltip("Path from Assets/ to the Excel file to import(Use forward slashs '/')")]
    public string excelFilePath = "Editor/ItemTree.xlsx";
    public string SaveItemsPath = "Assets/Items/";
    public string ItemTexturePath = "Assets/Textures/Items/";
    public string TableName;

    public AllItems ItemList;

    [ContextMenu("Import")]
    public void Import()
    {
        //Reset list of objects
        ItemList.AllGameItems.Clear();

        var excel = new ExcelImporter(excelFilePath);

        ImportItems(TableName, excel);

        //Find all Gameobjects in the Items folder
        string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/Items" });
        foreach (string guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject newItem = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            ItemList.AllGameItems.Add(newItem.GetComponent<GameItem>());
        }

        ImportMadeOf(TableName, excel);
    }

    /// <summary>
    /// Find a table in the imported excel file and create a new prefab with the data.
    /// </summary>
    void ImportItems(string TableName, ExcelImporter excel)
    {
        //Find table in the excel file by name and save as refrence 'table'
        //If the table cannot be found, stop this method
        if(!excel.TryGetTable(TableName, out var table))
        {
            Debug.LogError($"Table {TableName} NOT FOUND!");
            return;
        }

        //Iterate through all rows in the table
        for(int row = 1; row <= table.RowCount; row++)
        {
            //Find and save the "Name" row of the table
            string name = table.GetValue<string>(row, "Name");

            if (string.IsNullOrWhiteSpace(name)) continue;

            int Tier = table.GetValue<int>(row, "Tier");
            string Description = table.GetValue<string>(row, "Description");

            CreatePrefab(name, Tier, Description);          
        }
    }

    /// <summary>
    /// Find all items in the items folder and update all of their madeofs
    /// </summary>
    void ImportMadeOf(string TableName, ExcelImporter excel)
    {
        //If the table cannot be found, stop this method
        if (!excel.TryGetTable(TableName, out var table))
        {
            Debug.LogError($"Table {TableName} NOT FOUND!");
            return;
        }

        for (int row = 1; row <= table.RowCount; row++)
        {
            //Find the "Name" row of the table
            string nameOnTable = table.GetValue<string>(row, "Name");

            if (string.IsNullOrWhiteSpace(nameOnTable)) continue;

            //Find the name of the object that is being modified
            GameItem item = FindItemByName(nameOnTable);
            item.MadeOf.Clear();
            if (item != null)
            {
                //how many component 1 to add?
                int Component1Count = table.GetValue<int>(row, "Component 1 count");
                if (Component1Count > 0)
                {
                    for (int i = 0; i < Component1Count; i++)
                    {
                        //Add 1 of the component, based on name matches.
                        item.MadeOf.Add(FindItemByName(table.GetValue<string>(row, "Component 1")));
                    }
                }
                //how many component 2 to add?
                int Component2Count = table.GetValue<int>(row, "Component 2 count");
                if (Component2Count > 0)
                {
                    for (int i = 0; i < Component2Count; i++)
                    {
                        //Add 1 of the component, based on name matches.
                        item.MadeOf.Add(FindItemByName(table.GetValue<string>(row, "Component 2")));
                    }
                }
            }
        }

        GameItem FindItemByName(string ItemName)
        {
            //iterate through all items and find a name that matches the name on the table.
            foreach (GameItem item in ItemList.AllGameItems)
            {
                if (item.ItemName == ItemName)
                {
                    return item;
                }
            }
            Debug.LogError($"No Item of the name {ItemName}");
            return null;
        }

    }

    public void CreatePrefab(string name, int Tier, string Description)
    {
        //Create a temporary empty gameobject and keep a refrence of it
        GameObject TempItem = new GameObject(name);
        //Save the temporary object as a prefab, keep it as a seperate refrence
        GameObject PrefabItem = PrefabUtility.SaveAsPrefabAsset(TempItem, SaveItemsPath + name + ".prefab");

        //!!! Destroy the temperary gameObject so it does not show up in scene(Or any other random places) !!!
        DestroyImmediate(TempItem);

        //Check if item already have component added, if not, add one.
        var itemSr = PrefabItem.GetComponent<SpriteRenderer>();
        itemSr = itemSr != null ? itemSr : PrefabItem.AddComponent<SpriteRenderer>();

        var itemVariable = PrefabItem.GetComponent<GameItem>();
        itemVariable = itemVariable != null ? itemVariable : PrefabItem.AddComponent<GameItem>();

        //Set variables on custom script with information in excel table.
        itemVariable.ItemName = name;
        itemVariable.Tier = Tier;
        itemVariable.Description = Description;
        ImportSprite(itemVariable, itemSr);
    }

    public void ImportSprite(GameItem item, SpriteRenderer sr)
    {
        //Find all imported sprites in the given path
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { ItemTexturePath });
        foreach (string guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite newSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

            if(newSprite.name == "Unknown")
            {
                item.Sprite = newSprite;
                sr.sprite = newSprite;
            }

            //!!! Find the sprite by name, if the sprite name does not match with itemName, this will not work. !!!
            if (item.ItemName == newSprite.name)
            {
                item.Sprite = newSprite;
                sr.sprite = newSprite;
                return;
            }
        }
        
    }
}

[CustomEditor(typeof(ItemImporter))]
public class ItemImportorEditor : Editor
{
    ItemImporter ItemImporter;
    public override void OnInspectorGUI()
    {
        ItemImporter = (ItemImporter)target;

        base.OnInspectorGUI();
        if(GUILayout.Button("Create Items"))
        {
            ItemImporter.Import();
        }
    }
}
