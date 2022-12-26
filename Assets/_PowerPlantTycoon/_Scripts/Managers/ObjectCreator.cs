using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;

public class ObjectCreator : BaseSingleton<ObjectCreator>
{
    [SerializeField] MoneyItem moneyCollectablePrefab;
    [SerializeField] List<CollectableByType> collectablePrefabs;
    [SerializeField] List<PrefabByProduct> prefabByRaws;
    public GameObject miningTouchVFX;
    public GameObject buildingVFX;





    public static CollectableProductItem createCollectableProduct(ProductType type)
    {
        CollectableByType collectableItem = instance.collectablePrefabs.Find(item => item.productType == type);
        return Instantiate(collectableItem.item);
    }

    public static MoneyItem createMoneyCollecatble()
    {
        return Instantiate(instance.moneyCollectablePrefab);
    }
    public void MinerTouchVFX(Vector3 pos)
    {
        GameObject go = Instantiate(miningTouchVFX, pos + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(go, 1f);
    }
    public void BuildingVFX(Vector3 pos)
    {
        GameObject go = Instantiate(buildingVFX, pos, Quaternion.identity);
        Destroy(go, 1f);
    }
}

[System.Serializable]
public class PrefabByProduct
{
    public ProductType productType;
}

[System.Serializable]
public class CollectableByType
{
    public ProductType productType;
    public CollectableProductItem item;
}