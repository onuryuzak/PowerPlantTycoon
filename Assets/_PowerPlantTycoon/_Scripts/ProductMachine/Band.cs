using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Band : MonoBehaviour
{
    [SerializeField] float _bandSpeed;
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _endPoint;
    [SerializeField] ProductIdType _productId;

    public UnityEvent<CoalItem> _onProductLeaveFromBand;

    public List<CoalItem> _productList = new List<CoalItem>();

    float _maxDistance => (_endPoint.position - _startPoint.position).magnitude;
    float speed = 0.2f;
    bool workOnlyEachCoal;
    ProductData productSO;
    int count;

    private void Start()
    {
        productSO = ProductDataManager.instance.getProductData(_productId);
        count = productSO.dropZoneProductCount;
    }

    public void onProductAddToBand(List<CoalItem> productList)
    {
        _productList = productList;
    }



    private void Update()
    {
        if (_productList.Count > 0)
        {
            Transform product = _productList[0].transform;
            if (!workOnlyEachCoal)
            {
                workOnlyEachCoal = true;
                product.position = _startPoint.position;
                product.GetComponent<Rigidbody>().isKinematic = true;
            }
            float currentRatio = Vector3.Distance(product.position, _startPoint.position) / _maxDistance;
            float nextRatio = Mathf.Clamp(currentRatio + Time.deltaTime * _bandSpeed / speed, 0, 1);

            product.position = Vector3.Lerp(_startPoint.position, _endPoint.position, nextRatio);
            product.rotation = Quaternion.LookRotation((_endPoint.position - _startPoint.position));

            if (nextRatio == 1)
            {
                workOnlyEachCoal = false;
                _onProductLeaveFromBand.Invoke(_productList[0]);
                Destroy(_productList[0].gameObject);
                _productList.Remove(_productList[0]);
                productSO.dropZoneProductCount--;
                ProductDataManager.instance.saveGame();
            }
        }


    }
}