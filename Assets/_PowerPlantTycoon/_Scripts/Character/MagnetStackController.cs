using Artera;
using System;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MagnetStackController : MonoBehaviour
{
    public List<CollectableItem> Collectables;
    public bool hasItem => Collectables.Count > 0;
    public int collectedCount => Collectables.Count;

    StackerHandler stackerHandler
    {
        get
        {
            if (_stackerHandler == null)
                _stackerHandler = new StackerHandler();

            return _stackerHandler;
        }
    }

    StackerHandler _stackerHandler;

    public void addItemToList(CollectableItem item)
    {
        stackerHandler.addItemToList(item);
    }

    public CollectableItem getLastStackedObject(CollectType collectType, StackingType stackingType)
    {
        return stackerHandler.getLastStackedObject(collectType, stackingType);
    }


}