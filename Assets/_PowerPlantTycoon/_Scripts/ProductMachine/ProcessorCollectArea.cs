using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

public class ProcessorCollectArea : MonoBehaviour
{
    [SerializeField] ProductIdType _productId;
    [SerializeField] StackingProductListSO _rawProductSO;
    [SerializeField] CollectType _collectionType;
    [SerializeField] UnityEvent<List<CoalItem>> _onProductReadyForBand;



    List<CoalItem> _collectedItems;
    float _lastCollectedTime = 0;
    float _collectionFrequency = .05f;
    bool workOneTime = false;
    ProductData productSO;

    private void Start()
    {
        _collectedItems = new List<CoalItem>();
        productSO = ProductDataManager.instance.getProductData(_productId);

        DOVirtual.DelayedCall(1, () =>
        {
            int count = productSO.dropZoneProductCount;
            applyInitCollectedProduct(count);
        });
    }

    void applyInitCollectedProduct(int count)
    {
        for (int i = 0; i < count; i++)
        {

            CollectableItem item = ObjectCreator.createCollectableProduct(_rawProductSO.productType);
            if (item != null)
            {
                Rigidbody itemRB = item.GetComponent<Rigidbody>();
                item.transform.SetParent(gameObject.transform);
                item.gameObject.layer = LayerMask.NameToLayer("Default");
                itemRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

                item.transform.localPosition = new Vector3(Random.Range(-1.25f, 1.25f), -0.35f, Random.Range(-1f, 1f));
                item.GetComponent<CoalItem>().canCollect = false;
                if (_collectionType == CollectType.Coal)
                    item.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

                _collectedItems.Add(item.GetComponent<CoalItem>());
                _rawProductSO.stackingItems.Add(item.transform);


            }
        }
        DOVirtual.DelayedCall(1f, () =>
        {
            for (int i = 0; i < _collectedItems.Count; i++)
            {

                _collectedItems[i].gameObject.layer = LayerMask.NameToLayer("Coal");
                //_collectedItems[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                //_collectedItems[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        });
        _onProductReadyForBand?.Invoke(_collectedItems);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("CarryAI"))
        {
            
            if ((Time.time - _lastCollectedTime) < _collectionFrequency)
                return;

            for (int i = 0; i < 3; i++)
            {
                MagnetStackController stackController = other.GetComponentInChildren<MagnetStackController>();
                if (stackController)
                {
                    CollectableItem item = stackController.getLastStackedObject(_collectionType, StackingType.Magnet);
                    if (item != null)
                    {
                        if (!workOneTime)
                        {
                            workOneTime = true;
                            if (!InventoryManager.instance._inventorySO.tutorialIsDone)
                            {
                                TutorialSystem.instance.UpgradeTargetIndex();
                                TutorialSystem.instance.UpgradeTextIndex();
                                BuildingManager.instance.TutorialInitWire(1);
                            }
                        }
                        item.GetComponent<Rigidbody>().isKinematic = false;
                        item.onCollected(new Vector3(Random.Range(-1.25f, 1.25f), -0.2f, Random.Range(-1f, 1f)), gameObject.transform, 1);

                        _collectedItems.Add(item.GetComponent<CoalItem>());
                        _rawProductSO.stackingItems.Add(item.transform);

                        productSO.dropZoneProductCount++;
                    }


                }
            }
            GameManager.instance.player.StackIndexer = 0;
            GameManager.instance.player.YOffset = 0f;
            if (GameManager.instance._carryAIController != null)
            {
                GameManager.instance._carryAIController.StackIndexer = 0;
                GameManager.instance._carryAIController.YOffset = 0f;

            }

            ProductDataManager.instance.saveGame();
        }

    }



    private void OnApplicationQuit()
    {
        _rawProductSO.stackingItems.Clear();
    }
}