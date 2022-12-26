using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingAreaThrowPrice", menuName = "Data/BuildingAreaThrowPrice")]
public class BuildingAreaThrowPriceSO : ScriptableObject
{
    public List<BuildingThrowMoney> moneyPrices;
    public int ThrowPriceByType(BuildingType buildingId)
    {
        return moneyPrices.Find(item => item.type == buildingId).price;
    }
}
[System.Serializable]
public class BuildingThrowMoney
{
    public BuildingType type;
    public int price;
}
