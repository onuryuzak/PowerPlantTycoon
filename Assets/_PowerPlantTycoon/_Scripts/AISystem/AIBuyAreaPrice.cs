using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;

[CreateAssetMenu(fileName = "AIBuyAreaPrice", menuName = "Artera/AIBuyAreaPrice")]
public class AIBuyAreaPrice : BaseScriptableObject
{
    public List<AIAreaPrice> aiBuyPrices;
    public int getPriceByType(AIType aiId)
    {
        return aiBuyPrices.Find(item => item.type == aiId).price;
    }
}



[System.Serializable]
public class AIAreaPrice
{
    public AIType type;
    public int price;
}
