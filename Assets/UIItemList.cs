using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemList : MonoBehaviour
{
    public List<UIItem> UI_Items;

    public List<GameItem> ItemsList;

    public TypeFilter TypeFilter;
    public TierFilter TierFilter;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize all Item Slots
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot newSlot = transform.GetChild(i).GetComponent<Slot>();
            if (newSlot != null)
            {
                var newUIItem = newSlot.GetComponentInChildren<UIItem>();
                UI_Items.Add(newUIItem.GetComponentInChildren<UIItem>());
            }
        }

        ItemsList = ItemFilter.FilteredItems(TypeFilter, TierFilter);
    }
}
