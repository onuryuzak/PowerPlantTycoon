using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FactoryController : MonoBehaviour
{
    [SerializeField] InventorySO _inventorySO;
    [SerializeField] CoalItem _coalItem;
    [SerializeField] Image _fillImage;
    [SerializeField] float _delayTime;
    [SerializeField] float _decreaseTime;
    [SerializeField] float _increaseTime;
    [SerializeField] private GameObject smokeFactoryVFX;
    [SerializeField] private GameObject smokeFlueVFX;
    public float factoryEnergy;
    public bool WireConnectedSomeBuilding;
    Band _band;
    Tween _fillTween;
    Tween _decreaseEnergyTween;
    int count = 1;
    int _buildingIndex => InventoryManager.instance.buildingIndex;

    private void Start()
    {
        factoryEnergy = _inventorySO.factoryEnergy;
        _fillImage.fillAmount = _inventorySO.factoryFillAmount;
        _band = FindObjectOfType<Band>();
        StartCoroutine(CheckIfProductFinish());
        smokeFactoryVFX.SetActive(false);
        smokeFlueVFX.SetActive(false);
    }

    private void Update()
    {
        if (factoryEnergy > 0.01f)
        {
            smokeFactoryVFX.SetActive(true);
            smokeFlueVFX.SetActive(true);
        }
        else
        {
            smokeFactoryVFX.SetActive(false);
            smokeFlueVFX.SetActive(false);
        }
    }


    IEnumerator CheckIfProductFinish()
    {
        yield return new WaitUntil(() => _band._productList.Count == 0);

        _fillTween = _fillImage.DOFillAmount(0, _decreaseTime / (_buildingIndex + 1)).SetDelay(_delayTime);
        if (factoryEnergy > 0.01f)
        {
            float targetEnergy = factoryEnergy - _coalItem.energyPoint;
            _decreaseEnergyTween = DOTween
                .To(x => factoryEnergy = x, factoryEnergy, 0, _decreaseTime / (_buildingIndex + 1)).SetDelay(_delayTime)
                .OnUpdate(() =>
                {
                    _inventorySO.factoryEnergy = factoryEnergy;
                    _inventorySO.factoryFillAmount = factoryEnergy;
                    Database.instance.saveGame();
                });
        }
    }

    public void AddEnergyToFactory(CoalItem currentCoal)
    {
        if (factoryEnergy <= 1)
        {
            if (_fillTween != null) _fillTween.Kill();
            if (_decreaseEnergyTween != null) _decreaseEnergyTween.Kill();
            factoryEnergy += currentCoal.energyPoint;
            _fillTween = _fillImage.DOFillAmount(factoryEnergy, _increaseTime).OnUpdate(() =>
            {
                _inventorySO.factoryEnergy = factoryEnergy;
                _inventorySO.factoryFillAmount = _fillImage.fillAmount;
                Database.instance.saveGame();
            });
        }


        if (_band._productList.Count - 1 == 0) StartCoroutine(CheckIfProductFinish());
    }
}