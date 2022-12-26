using Artera;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ProductDataManager : BaseSingleton<ProductDataManager>
{
    [SerializeField] ProductSaveSO _productSaveSO;
    List<ProductData> _list => _productSaveSO.getProductList;
    Tween _saveTween;
    public ProductData getProductData(ProductIdType id)
    {
        return _list.Find(item => item.productId == id);
    }

    public void saveGame()
    {
        if (_saveTween != null)
            _saveTween.Kill();

        _saveTween = DOVirtual.DelayedCall(.1f, () =>
        {
            Database.instance.saveGame();
            _saveTween = null;
        });
    }
}