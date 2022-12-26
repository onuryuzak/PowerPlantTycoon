using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;

public class PowerUpManager : BaseSingleton<PowerUpManager>
{
    public PlayerPowerUpSO _playerPowerUpSO;
    public float playerSpeedIncreaseValue;
    public int playerCoalMiningCountIncreaseValue;
    public int getAreaPrice(PowerUpIdType powerUpId)
    {
        return _playerPowerUpSO.getPriceByType(powerUpId);
    }

    public void onPowerUpUpgrade(PowerUpIdType powerUpIdType)
    {
        if (powerUpIdType == PowerUpIdType.Speed)
        {
            _playerPowerUpSO.PlayerSpeed += playerSpeedIncreaseValue;
            _playerPowerUpSO.setPriceByType(powerUpIdType);

        }
        if (powerUpIdType == PowerUpIdType.CoalPiece)
        {
            _playerPowerUpSO.CoalMiningCount += playerCoalMiningCountIncreaseValue;
            _playerPowerUpSO.setPriceByType(powerUpIdType);
        }
    }
}
