using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;

[CreateAssetMenu(fileName = "PlayerPowerUp", menuName = "Artera/PlayerPowerUp")]
public class PlayerPowerUpSO : BaseScriptableObject
{
    public int CoalMiningCount;
    public float PlayerSpeed;
    public int priceIncreaseValue;

    public List<PowerAreaPrice> powerUpPrices;

    public int getPriceByType(PowerUpIdType powerUpId)
    {
        return powerUpPrices.Find(item => item.type == powerUpId).price;
    }
    public void setPriceByType(PowerUpIdType powerUpId)
    {
        powerUpPrices.Find(item => item.type == powerUpId).price += priceIncreaseValue;

    }

    public override void reset()
    {
        CoalMiningCount = 2;
        PlayerSpeed = 5f;
        foreach (var item in powerUpPrices)
        {
            item.price = 10;
        }
    }
}

[System.Serializable]
public class PowerAreaPrice
{
    public PowerUpIdType type;
    public int price;
}
