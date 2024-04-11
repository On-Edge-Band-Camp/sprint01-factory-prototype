using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Dictionary<string, object>> gameRecipes = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> itemInport = new List<Dictionary<string, object>>();

    public Item[] items;

    private Item[] inputsForThisItem = new Item[2];

    public GameItem EmptyItem;

    public AllItems ItemList;
    public List<GameItem> AllGameItems;

    public GameObject PauseMenu;
    bool pauseActive;

    public Tooltip Tooltip;
    public Tooltip MachineTooltip;

    private void Awake()
    {
        gameRecipes = CSVReader.Read("Recipes");
        itemInport = CSVReader.Read("Items");

        AllGameItems.Clear();
        LoadItems();

        ItemConstructor();
    }

    private void Start()
    {
        Tooltip.gameObject.SetActive(false);
        MachineTooltip.gameObject.SetActive(false);
    }

    private void Update()
    {
        RuntimeTelemtry();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseActive = !pauseActive;
            PauseMenu.SetActive(pauseActive);
        }

        if (pauseActive)
        {
            Time.timeScale = 0;
        }

        if (pauseActive)
        {
            Time.timeScale = 1;
        }
    }

    [ContextMenu("LoadItems")]
    public void LoadItems()
    {
        foreach (GameItem item in ItemList.AllGameItems)
        {
            AllGameItems.Add(item);
        }
    }

    public void Resume()
    {
        pauseActive = false;
        PauseMenu.SetActive(pauseActive);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void RuntimeTelemtry()
    {
        if (Input.GetButtonDown("Rotate"))
        {
            TelemetryLogger.Log(this, "RotateAttempt");
        }
    }

    //This function assembles the list of items for mechines and UI to use
    void ItemConstructor()
    {
        //Makes an item for every item in the Items spreadsheet
        items = new Item[itemInport.Count];

        //fills in all feilds except inputs item list. This is bc we need all of the items made before we can put inputs in a list
        for (int i = 0; i < itemInport.Count; i++)
        {
            string[] inputNames = new string[] { itemInport[i]["Input1"].ToString(), itemInport[i]["Input2"].ToString() };

            items[i] = new Item(itemInport[i]["Index"], itemInport[i]["Name"].ToString(), inputNames, itemInport[i]["Sprite"].ToString(), itemInport[i]["Input1Ratio"], itemInport[i]["Input2Ratio"]);
        }

        //This takes the string names of the input items and finds thier Item and puts them in the proper array
        for (int i = 0; i < items.Length; i++)
        {
            items[i].inputRatios = new int[] { Convert.ToInt32(items[i].Input1Ratio), Convert.ToInt32(items[i].Input2Ratio) };

            for (int j = 0; j < items.Length; j++)
            {
                if (items[i].inputNames[0] == items[j].Name)
                {
                    inputsForThisItem[0] = items[j];
                }
                if (items[i].inputNames[1] == items[j].Name)
                {
                    inputsForThisItem[1] = items[j];
                }
            }
            items[i].inputs = inputsForThisItem;
            inputsForThisItem = new Item[2];
        }
    }

    //Will return the class of any itemName given, will return null if it cannot be found OUTDATED DON NOT USE
    public Item findItemByName(string itemName)
    {
        for (int i = 0;i < items.Length;i++)
        {
            if (items[i].Name == itemName)
            {
                return items[i];
            }
        }
        Debug.LogWarning("Item dosen't exist. Check for any Typos in any input fields");
        return null;
    }

    //This is the class for all item information OUTDATED DO NOT USE
    public class Item
    {
        public object index;
        public string Name;
        public Item[] inputs;
        public Sprite sprite;
        public int[] inputRatios;
        public object Input1Ratio;
        public object Input2Ratio;
        public int outputRatio;

        public string spriteName;

        //The inputNames veriable is not to be used outside of the constructor. Use inputs instead
        public string[] inputNames;
        public Item(object index, string name, string[] inputs, string sprite, object input1Ratio, object input2Ratio)
        {
            this.index = index;
            this.Name = name;
            this.inputNames = inputs;
            this.spriteName = sprite;
            this.sprite = null;
            this.inputs = new Item[2];
            this.Input1Ratio = input1Ratio;
            this.Input2Ratio = input2Ratio;
            this.inputRatios = new int[2];
        }
    }
}
