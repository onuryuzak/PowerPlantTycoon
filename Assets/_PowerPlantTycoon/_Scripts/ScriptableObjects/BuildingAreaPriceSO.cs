using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingAreaPrice", menuName = "Data/BuildingAreaPrice")]
public class BuildingAreaPriceSO : ScriptableObject
{
    public List<ProductAreaPrice> productPrices;

    public int priceByType(BuildingType buildingId)
    {
        return productPrices.Find(item => item.type == buildingId).price;
    }
}
[System.Serializable]
public class ProductAreaPrice
{
    public BuildingType type;
    public int price;
}
