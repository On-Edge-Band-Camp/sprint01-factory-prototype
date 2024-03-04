using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ItemImporter.Asset", menuName ="Data/Item Importer")]
public class ItemImporter : ScriptableObject
{
    [Tooltip("Path from Assets/ to the Excel file to import(Use forward slashs '/')")]
    public string excelFilePath = "Editor/ItemTree.xlsx";
    public string TableName;
    public List<Items> AllItems;

    [ContextMenu("Import")]
    public void Import()
    {
        //Reset list of objects
        AllItems.Clear();

        var excel = new ExcelImporter(excelFilePath);

        var items = DataHelper.GetAllAssetsOfType<Items>();

        ImportItems(TableName, excel, items);

        //Find all items in the Items folder
        string[] guids = AssetDatabase.FindAssets("t:Items", new[] { "Assets/Items" });
        foreach (string guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            Items newItem = AssetDatabase.LoadAssetAtPath<Items>(path);
            Debug.Log(newItem.ItemName);
            AllItems.Add(newItem);
        }

        ImportMadeOf(TableName, excel, items);
    }

    void ImportItems(string TableName, ExcelImporter excel, Dictionary<string, Items> items)
    {
        //If the table cannot be found, stop this method
        if(!excel.TryGetTable(TableName, out var table))
        {
            Debug.LogError($"Table {TableName} NOT FOUND!");
            return;
        }

        for(int row = 1; row <= table.RowCount; row++)
        {
            //Find the "Name" row of the table
            string name = table.GetValue<string>(row, "Name");
            Debug.Log(name + " is being created");

            if (string.IsNullOrWhiteSpace(name)) continue;

            var item = DataHelper.GetOrCreateAsset(name, items, TableName);

            if (string.IsNullOrWhiteSpace(item.ItemName))
            {
                item.ItemName = name;
            }

            item.Tier = table.GetValue<int>(row, "Tier");
        }
    }

    /// <summary>
    /// Find all items in the items folder and update all of their madeofs
    /// </summary>
    void ImportMadeOf(string TableName, ExcelImporter excel, Dictionary<string, Items> items)
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
            Debug.Log(nameOnTable + " is being edited");

            if (string.IsNullOrWhiteSpace(nameOnTable)) continue;

            //Find the name of the object that is being modified
            Items item = FindItemByName(nameOnTable);
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

        Items FindItemByName(string ItemName)
        {
            //iterate through all items and find a name that matches the name on the table.
            foreach (Items item in AllItems)
            {
                if (item.name == ItemName)
                {
                    return item;
                }
            }
            Debug.LogError($"No Item of the name {ItemName}");
            return null;
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
