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
        var collector = machineDetails.machine.GetComponent<Collector>();
        if (collector != null)
        {
            collector.CollectingItem = Item;
            SetUIItemInTarget(machineDetails.ItemSelectionUI);
            DontCloseOnClick();
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
