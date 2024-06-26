using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TypeFilter {None, All, CraftableItems, UncraftableItems}

[Flags]
public enum TierFilter 
{
    None = 0,
    Tier0 = 1,
    Tier1 = 2, 
    Tier2 = 4,
    Tier3 = 8,
    Tier4 = 16,
    Tier5 = 32
}

public class ItemFilter
{
    public GameManager GM;

    /// <summary>
    /// Input the filter to returns a array of int representing tiers
    /// </summary>
    public static List<int> FilteredTierList(TierFilter tierFilter)
    {
        List<int> TierList = new List<int>();

        if (tierFilter.HasFlag(TierFilter.Tier0))
        {
            TierList.Add(0);
        }

        if (tierFilter.HasFlag(TierFilter.Tier1))
        {
            TierList.Add(1);
        }

        if (tierFilter.HasFlag(TierFilter.Tier2))
        {
            TierList.Add(2);
        }

        if (tierFilter.HasFlag(TierFilter.Tier3))
        {
            TierList.Add(3);
        }

        if (tierFilter.HasFlag(TierFilter.Tier4))
        {
            TierList.Add(4);
        }

        if (tierFilter.HasFlag(TierFilter.Tier5))
        {
            TierList.Add(5);
        }
        return TierList;
    }

    /// <summary>
    /// Use filters to get a filtered list of items
    /// </summary>
    public static List<GameItem> FilteredItems(List<GameItem> UnfilteredList, TypeFilter typeFilter, TierFilter tierFilter)
    {
        List<int> FilteredTiers = FilteredTierList(tierFilter);

        List<GameItem> FilteringList = new List<GameItem>();
        foreach(GameItem item in UnfilteredList)
        {
            FilteringList.Add(item);
        }

        List<GameItem> UnqualifyItems = new List<GameItem>();

        if (typeFilter == TypeFilter.None)
        {
            Debug.Log("Type filter is set to none, no item added to list");
            return null;
        }

        foreach (GameItem item in FilteringList)
        {
            Debug.Log($"Filtering {item}");
            //If the type does not match or tier does not match, remove this item
            if (!FilterByType(item,typeFilter) || !FilterByTier(item, FilteredTiers))
            {
                UnqualifyItems.Add(item);
            }
        }

        foreach(GameItem item in UnqualifyItems)
        {
            FilteringList.Remove(item);
        }
        return FilteringList;
    }

    public static bool FilterByType(GameItem item, TypeFilter typeFilter)
    {
        Debug.Log("Filtering by Type");
        //Add all filtered items in list
        switch (typeFilter)
        {
            case TypeFilter.None:
                return false;
            case TypeFilter.All:
                return true;
            case TypeFilter.CraftableItems:
                Debug.Log($"Checking {item}, have {item.MadeOf.Count} components");
                if (item.MadeOf.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case TypeFilter.UncraftableItems:
                if (item.MadeOf.Count <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }

    public static bool FilterByTier(GameItem item ,List<int> tierFilters)
    {
        if (tierFilters.Count > 0)
        {
            if (tierFilters.Contains(item.Tier))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
