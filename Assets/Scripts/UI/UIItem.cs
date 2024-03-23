using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItem : MonoBehaviour
{
    public MachineDetails machineDetails;
    public GameItem Item;
    public Image image;
    public Animator ac;
    public TMP_Text text;

    public GameObject ToolTip;
    public TMP_Text ToolTipName;
    public TMP_Text TooltipDescription;


    public bool HaveInitialItem;
    // Start is called before the first frame update
    void Start()
    {
        ToolTip.SetActive(false);
        if (HaveInitialItem)
        {
            switch (machineDetails.machine.machine_type)
            {
                case machine_types.Collector:
                    //Get item from collector's collecting item.
                    Collector collector = machineDetails.machine.GetComponent<Collector>();
                    var CollectingItem = collector.CollectingItem;
                    if (CollectingItem != null)
                    {
                        SetItem(CollectingItem);
                    }
                    break;
                case machine_types.Constructor:
                    //Get item from collector's collecting item.
                    Constructor constructor = machineDetails.machine.GetComponent<Constructor>();
                    var FinalProduct = constructor.finalProduct;
                    if (FinalProduct != null)
                    {
                        SetItem(FinalProduct);
                        SetComponents(FinalProduct);
                    }

                    //Find Components

                    break;
            }
        }

        if (Item != null)
        {
            image.sprite = Item.Sprite;
        }
        else
        {
            Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowToolTip()
    {
        if(ToolTip!=null && Item != null)
        {
            ToolTip.SetActive(true);
        }
    }

    public void HideToolTip()
    {
        ToolTip.SetActive(false);
    }

    public void SetItem(GameItem item)
    {
        Clear();
        image.enabled = true;
        Item = item;
        image.sprite = item.Sprite;
        if(ToolTip != null)
        {
            ToolTipName.text = item.ItemName;
            TooltipDescription.text = item.Description;
        }
    }

    public void updateCount(int itemCount)
    {
        if (itemCount <= 0)
        {
            Clear();
        }
        else
        {
            text.text = itemCount.ToString();
            Pop();
        }
    }

    public void Pop()
    {
        ac.Play("Pop");
    }

    public void BigPop()
    {
        ac.Play("BigPop");
    }

    public void Clear()
    {
        Item = null;
        image.sprite = null;
        image.enabled = false;
        text.text = " ";
    }
    /// <summary>
    /// Temerpory. upgrade to fit all machines later.
    /// </summary>
    /// 

    public void SetItemInMachine()
    {
        machine_types MachineType = machineDetails.machine.machine_type;
        switch (MachineType)
        {
            case machine_types.Collector:
                DontCloseOnClick();
                var collector = machineDetails.machine.GetComponent<Collector>();
                collector.CollectingItem = Item;
                SetUIItemInTarget(machineDetails.ItemSelectionUI);
                break;

            case machine_types.Constructor:
                DontCloseOnClick();
                SetConstructorRecipe();
                break;
        }  
    }

    public void SetUIItemInTarget(UIItem Target)
    {
        Target.SetItem(Item);
    }

    public void DontCloseOnClick()
    {
        machineDetails.gameObject.GetComponent<MachineSelectMenu>().DontCloseOnClick();
    }

    /// <summary>
    /// Used by buttons to set the reciepe of constructor;
    /// </summary>
    public void SetConstructorRecipe()
    {
        var construtor = machineDetails.machine.GetComponent<Constructor>();
        construtor.finalProduct = Item;
        SetUIItemInTarget(machineDetails.ItemSelectionUI);
        SetComponents(Item);
    }

    public void SetComponents(GameItem CraftableItem)
    {
        //Set components
        //Find all componets of a different type;
        List<GameItem> ItemTypes = new List<GameItem>();
        foreach (var item in CraftableItem.MadeOf)
        {
            if (!ItemTypes.Contains(item))
            {
                ItemTypes.Add(item);
                Debug.Log(item);
            }
        }
        //Find all UI item slot.
        foreach (Transform child in machineDetails.ItemSelectionUI.transform)
        {
            var ComponentUIItem = child.GetComponentInChildren<UIItem>();
            if (ComponentUIItem != null)
            {
                ComponentUIItem.SetItem(ItemTypes[0]);
                ComponentUIItem.updateCount(CraftableItem.ItemCount(ItemTypes[0]));

                ItemTypes.Remove(ItemTypes[0]);
            }
        }
    }
}
