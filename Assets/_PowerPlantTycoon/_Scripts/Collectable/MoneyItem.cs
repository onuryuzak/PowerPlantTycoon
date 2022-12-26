using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyItem : CollectableItem
{
    public ProductIdType belongProductId { get; set; }
    public int moneyValue { get; set; }
    Rigidbody _ridigBody;
    public float timeLeft = 2f;

    protected override void Start()
    {
        canCollect = true;
        _ridigBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (!collected && canCollect)
        {
            float playerDistance = (GameManager.instance.player.transform.position - transform.position).magnitude;

            if (playerDistance < distanceToCollect)
            {
                GameManager.instance.player.collectMe(this, StackingType.Magnet);
            }
            
        }
    }

    public override void onCollected(Vector3 targetPoint, Transform parent = null, float finalScaleFactor = 1, Action onCompleted = null)
    {
        _ridigBody.isKinematic = true;
        collected = true;
        transform.SetParent(parent);
        ProductDataManager.instance.saveGame();

        transform.DOLocalJump(targetPoint, 2, 1, .4f).OnComplete(() =>
        {
            onCompleted?.Invoke();
        });
    }
}
