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

    public bool HaveInitialItem;
    // Start is called before the first frame update
    void Start()
    {
        if (HaveInitialItem)
        {
            switch (machineDetails.machine.machine_type)
            {
                case machine_types.Collector:
                    //Get item from collector's collecting item.
                    Collector collector = machineDetails.machine.GetComponent<Collector>();
                    var CollectingItem = collector.CollectingItem;
                    Item = CollectingItem;
                    break;
            }
        }

        if (Item != null)
        {
            image.sprite = Item.Sprite;
            text.text = " ";
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

    public void SetItem(GameItem item)
    {
        image.enabled = true;
        Item = item;
        image.sprite = item.Sprite;
    }

    public void updateCount(int itemCount)
    {
        text.text = itemCount.ToString();
        Pop();
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
                var collector = machineDetails.machine.GetComponent<Collector>();
                collector.CollectingItem = Item;
                SetUIItemInTarget(machineDetails.ItemSelectionUI);
                DontCloseOnClick();
                break;
            case machine_types.Constructor:
                var construtor = machineDetails.machine.GetComponent<Constructor>();
                construtor.finalProduct = Item;
                SetUIItemInTarget(machineDetails.ItemSelectionUI);
                DontCloseOnClick();
                //Set components
                //Find all componets of a different type;
                List<GameItem> ItemTypes = new List<GameItem>();
                foreach(var item in construtor.finalProduct.MadeOf)
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
                    if(ComponentUIItem != null)
                    {
                        ComponentUIItem.SetItem(ItemTypes[0]);
                        ComponentUIItem.updateCount(construtor.finalProduct.ItemCount(ItemTypes[0]));
                        
                        ItemTypes.Remove(ItemTypes[0]);
                    }
                }
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
}
