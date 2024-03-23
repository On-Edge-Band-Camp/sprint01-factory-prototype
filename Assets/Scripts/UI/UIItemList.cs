using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemList : MonoBehaviour
{
    GameManager GM;
    public MachineDetails machineDetails;
    public List<UIItem> UI_Items;

    public List<GameItem> ItemsList;

    public TypeFilter TypeFilter;
    public TierFilter TierFilter;
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        //Initialize all Item Slots
        for (int i = 0; i < transform.childCount; i++)
        {
            Slot newSlot = transform.GetChild(i).GetComponent<Slot>();
            if (newSlot != null)
            {
                var newUIItem = newSlot.GetComponentInChildren<UIItem>();
                UI_Items.Add(newUIItem.GetComponentInChildren<UIItem>());
                newUIItem.machineDetails = machineDetails;
                newUIItem.Clear();
            }
        }

        ItemsList = ItemFilter.FilteredItems(GM.AllGameItems,TypeFilter, TierFilter);

        for(int i = 0; i < UI_Items.Count; i++)
        {
            Debug.Log(i);
            if (UI_Items[i].Item == null)
            {
                if(i< ItemsList.Count)
                {
                    UI_Items[i].SetItem(ItemsList[i]);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
