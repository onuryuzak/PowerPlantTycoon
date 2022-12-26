using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableItem : MonoBehaviour
{
    public CollectType collectType;
    public float distanceToCollect;
    public StackingType stackingType { get; set; }

    public bool collected = false;
    public bool canCollect = false;



    protected virtual void Start()
    {

    }

    public abstract void onCollected(Vector3 targetPoint, Transform parent = null, float finalScaleFactor = 1, System.Action onCompleted = null);

    public virtual void onRelease(Transform parent = null) { }

    public virtual void destroyYourSelf()
    {
        Destroy(gameObject);
    }
}