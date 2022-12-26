using Artera;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductSave", menuName = "Artera/Data/ProductSave")]
public class ProductSaveSO : BaseScriptableObject
{
    public List<ProductData> _saveableProductList;

    List<ProductIdType> _productIdList = new List<ProductIdType>() { ProductIdType.Coal_1 };

    public List<ProductData> getProductList
    {
        get
        {
            if (!isListOk())
                reset();

            return _saveableProductList;
        }
    }

    public override void reset()
    {
        _saveableProductList.Clear();
        for (int i = 0; i < _productIdList.Count; i++)
        {
            _saveableProductList.Add(new ProductData().initData(_productIdList[i]));
        }
    }

    bool isListOk()
    {
        bool isCool = true;

        if (_saveableProductList.Count == 0 || _saveableProductList.Count > _productIdList.Count)
            return false;

        foreach (ProductData p in _saveableProductList)
        {
            if (p == null)
            {
                isCool = false;
                break;
            }
        }

        return isCool;
    }
}

[System.Serializable]
public class ProductData
{
    public ProductIdType productId;
    public int dropZoneProductCount;
    public float speed;

    public ProductData initData(ProductIdType productId)
    {
        this.productId = productId;
        dropZoneProductCount = 0;
        speed = 0;

        return this;
    }
}