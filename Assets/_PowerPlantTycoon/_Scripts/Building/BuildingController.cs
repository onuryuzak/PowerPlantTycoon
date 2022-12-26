using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private float _colorChangeTweenTimer = 1.5f;
    [SerializeField] private Material _buildingWallsMat;
    [SerializeField] private Material _buildingRoofMat;
    [SerializeField] private Material _buildingWindowMat;
    [SerializeField] GameObject _moneyPrefab;
    [SerializeField] ProductIdType _productId;
    [SerializeField] BuildingType _buildingType;
    [SerializeField] BuildingAreaThrowPriceSO _buildingAreaThrowPriceSO;
    [SerializeField] private Color32 _targetBuildingWallsColor;
    [SerializeField] private Color32 _targetBuildingRoofColor;
    [SerializeField] private Color32 _targetBuildingWindowColor;
    [SerializeField] FloatingText _floatingTextPrefab;
    bool isAvailableThrowMoney;
    public float timeLeft = 0f;

    ProductData productSO;
    int _pricePerProduct;
    public bool thereIsEnergy;
    private bool workOneTime;

    private void OnEnable()
    {
        if (InventoryManager.instance._inventorySO.factoryEnergy == 0)
        {
            _buildingWallsMat.DOColor(Color.gray, _colorChangeTweenTimer);
            _buildingRoofMat.DOColor(Color.gray, _colorChangeTweenTimer);
            _buildingWindowMat.DOColor(Color.gray, _colorChangeTweenTimer);
        }
    }

    private void Start()
    {
        _pricePerProduct = _buildingAreaThrowPriceSO.ThrowPriceByType(_buildingType);
        productSO = ProductDataManager.instance.getProductData(_productId);
    }

    private void Update()
    {
        if (InventoryManager.instance.factoryEnergy > 0.01f && isAvailableThrowMoney)
        {
            thereIsEnergy = true;
            SetTargetColor();
        }
        else
        {
            thereIsEnergy = false;
            SetGrayColor();
        }

        if (isAvailableThrowMoney && thereIsEnergy)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                throwMoney();
                timeLeft = 2f;
            }
        }
    }

    private void throwMoney()
    {
        // Rigidbody rb = Instantiate(_moneyPrefab, _moneyPos.position, Quaternion.identity).GetComponent<Rigidbody>();
        // rb.transform.SetParent(transform);
        // rb.AddForce(_moneyPos.forward, ForceMode.VelocityChange);
        // rb.AddTorque(Vector3.one * 20, ForceMode.VelocityChange);

        MoneyItem money = _moneyPrefab.GetComponent<MoneyItem>();
        money.moneyValue = _pricePerProduct;
        money.belongProductId = _productId;
        InventoryManager.instance.addMoney(money.moneyValue);
        FloatingText floatingText = Instantiate(_floatingTextPrefab);
        floatingText.transform.position = (Vector3.right * 0.5f) + transform.position + (Vector3.up * 2f);
        floatingText.setText(money.moneyValue);

        ProductDataManager.instance.saveGame();
    }

    public void OnElectricConnected()
    {
        isAvailableThrowMoney = true;
        
        FindObjectOfType<FactoryController>().WireConnectedSomeBuilding = true;
    }

    public void SetGrayColor()
    {
        _buildingWallsMat.color =
            Color32.Lerp(_buildingWallsMat.color, Color.gray, Time.deltaTime * _colorChangeTweenTimer);
        //_buildingWallsMat.DOColor(Color.gray, _colorChangeTweenTimer);
        //_buildingRoofMat.DOColor(Color.gray, _colorChangeTweenTimer);
        _buildingRoofMat.color =
            Color32.Lerp(_buildingRoofMat.color, Color.gray, Time.deltaTime * _colorChangeTweenTimer);
        _buildingWindowMat.color =
            Color32.Lerp(_buildingWindowMat.color, Color.gray, Time.deltaTime * _colorChangeTweenTimer);
        //_buildingWindowMat.DOColor(Color.gray, _colorChangeTweenTimer);
    }

    public void SetTargetColor()
    {
        _buildingWindowMat.color =
            Color32.Lerp(_buildingWindowMat.color, _targetBuildingWindowColor, Time.deltaTime * _colorChangeTweenTimer);
        _buildingRoofMat.color =
            Color32.Lerp(_buildingRoofMat.color, _targetBuildingRoofColor, Time.deltaTime * _colorChangeTweenTimer);
        _buildingWallsMat.color =
            Color32.Lerp(_buildingWallsMat.color, _targetBuildingWallsColor, Time.deltaTime * _colorChangeTweenTimer);
        // _buildingWallsMat.DOColor(_targetBuildingWallsColor, _colorChangeTweenTimer);
        // _buildingRoofMat.DOColor(_targetBuildingRoofColor, _colorChangeTweenTimer);
        // _buildingWindowMat.DOColor(_targetBuildingWindowColor, _colorChangeTweenTimer);
        // _material.EnableKeyword("_EMISSION");
        // _material.SetColor("_EmissionColor", _color);
    }

    private void OnApplicationQuit()
    {
        SetGrayColor();
    }
}