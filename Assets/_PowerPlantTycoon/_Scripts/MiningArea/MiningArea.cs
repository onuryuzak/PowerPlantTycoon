using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MiningArea : MonoBehaviour
{
    bool workOneTime;
    [SerializeField] private float _range;
    [SerializeField] private Transform _randomTargetTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
        {
            EventManager.OnMiningAreaEnter(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
        {
            EventManager.OnMiningAreaExit();
        }
    }

    public void SpawnCoal(float Power)
    {
        Vector3 position = transform.position;
        ObjectCreator.instance.MinerTouchVFX(position);
        CollectableProductItem collectable = ObjectCreator.createCollectableProduct(ProductType.Coal);
        collectable.GetComponent<CoalItem>().canCollect = true;
        collectable.transform.position = position;
        collectable.transform.rotation = Quaternion.Euler(position);
        Vector3 randomPos = new Vector3(Random.insideUnitSphere.x, 0, Random.insideUnitSphere.z);
        Vector3 pos = _randomTargetTransform.position + randomPos * _range;
        
        collectable.transform.DOJump(pos, Random.Range(2f, 3f), 1, Random.Range(0.3f, 0.6f)).OnComplete((() =>
                collectable.GetComponent<Rigidbody>().velocity = randomPos * Power
            ));
    }
}