using UnityEngine;
using Artera;
using DG.Tweening;

public class InventoryManager : BaseSingleton<InventoryManager>
{
    public InventorySO _inventorySO;

    [Header("Header")]
    [SerializeField] MoneyChangeEventSO _moneyChangeEvent;

    Tween _saveTween;

    public int money => _inventorySO.money;
    public int buildingIndex => _inventorySO.buildingIndex;
    public int wireIndex => _inventorySO.wireIndex;
    public float factoryEnergy => _inventorySO.factoryEnergy;
    public void addMoney(int value)
    {
        _inventorySO.money += value;

        if (_inventorySO.money < 0)
            _inventorySO.money = 0;

        saveGame();

        _moneyChangeEvent.onMoneyChangeListener.Invoke(_inventorySO.money);
    }
    public void buildingIndexUpgrade()
    {
        _inventorySO.buildingIndex++;
        saveGame();
    }
    public void wireIndexUpgrade()
    {
        _inventorySO.wireIndex++;
        saveGame();
    }

    void saveGame()
    {
        if (_saveTween != null)
            _saveTween.Kill();

        _saveTween = DOVirtual.DelayedCall(.5f, () =>
        {
            
            Database.instance.saveGame();
            _saveTween = null;
        });
    }
}