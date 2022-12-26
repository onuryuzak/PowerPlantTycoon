using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoalItem : CollectableProductItem
{
    public ProductIdType belongProductId { get; set; }
    public float energyPoint = 0f;
    Rigidbody _ridigBody;

    protected override void Start()
    {
        _ridigBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!collected && canCollect)
        {
            float playerDistance = (GameManager.instance.player.MagnetModelHolder.transform.position - transform.position).magnitude;
            
            if (playerDistance < distanceToCollect)
            {
                GameManager.instance.player.collectMe(this, StackingType.Magnet);
            }
            if (GameManager.instance._carryAIController != null)
            {
                float AIDistance = (GameManager.instance._carryAIController.MagnetModelHolder.transform.position - transform.position).magnitude;
                if (AIDistance < distanceToCollect)
                {
                    GameManager.instance._carryAIController.collectMe(this, StackingType.Magnet);
                }
            }

        }
    }

    public override void onCollected(Vector3 targetPoint, Transform parent = null, float finalScaleFactor = 1, Action onCompleted = null)
    {
        collected = true;
        transform.SetParent(parent);
        canCollect = false;
        transform.DOLocalMove(targetPoint, .1f).OnComplete(() =>
        {
            onCompleted?.Invoke();
            _ridigBody.isKinematic = true;


        });
    }
    protected override float getHeightFromFloor()
    {
        return 2.5f;
    }
}
