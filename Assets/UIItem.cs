using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItem : MonoBehaviour
{
    public GameItem Item;
    public Image image;
    public Animator ac;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("SetItem");
        image.enabled = true;
        Item = item;
        image.sprite = item.Sprite;
    }

    public void updateCount(int itemCount)
    {
        Debug.Log("updateCount");

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

    public void SetItemInMachine(MachineDetails Target)
    {
        var collector = Target.machine.GetComponent<Collector>();
        if (collector != null)
        {
            collector.CollectingItem = Item;
        }       
    }

    public void SetUIItemInTarget(UIItem Target)
    {
        Target.SetItem(Item);
    }
}
