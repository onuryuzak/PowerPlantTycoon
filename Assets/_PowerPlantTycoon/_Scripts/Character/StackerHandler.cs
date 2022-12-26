using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerHandler
{
    public bool hasItem => _collectedWheelBarrowItems.Count > 0;
    public int itemCount => _collectedWheelBarrowItems.Count;


    List<CollectableItem> _collectedWheelBarrowItems = new List<CollectableItem>();
    public int currentFrontNumber = 0;
    CollectType collectType;
    StackingType lastStackingType;

    public int stackIndex
    {
        get
        {

            return currentFrontNumber;
        }
        set
        {
            if (lastStackingType == StackingType.Magnet)
            {
                currentFrontNumber = value;
                if (currentFrontNumber < 0)
                    currentFrontNumber = 0;
            }
        }
    }

    List<CollectableItem> collectedList(StackingType stackingType)
    {
            return _collectedWheelBarrowItems;

    }

    public void addItemToList(CollectableItem item)
    {
        this.collectType = item.collectType;
        collectedList(item.stackingType).Add(item);
    }

    public CollectType getCollectionType()
    {
        return CollectType.Coal;
    }

    public CollectableItem getLastStackedObject(CollectType collectType, StackingType stackingType)
    {
        List<CollectableItem> list = collectedList(stackingType);

        int lastIndex = list.Count - 1;

        if (lastIndex < 0)
            return null;

        CollectableItem lastItem = list[lastIndex];
        if (lastItem.collectType != collectType)
            return null;

        lastStackingType = lastItem.stackingType;
        this.collectType = collectType;
        list.RemoveAt(lastIndex);
        stackIndex--;

        return lastItem;
    }
}