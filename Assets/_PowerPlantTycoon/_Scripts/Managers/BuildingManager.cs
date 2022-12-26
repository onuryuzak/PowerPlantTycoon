using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;
using DG.Tweening;
using Obi;

public class BuildingManager : BaseSingleton<BuildingManager>
{
    [SerializeField] BuildingAreaPriceSO _productAreaSO;
    [SerializeField] BuildingArea[] _buildings;
    [SerializeField] ObiSolver[] _wires;
    [SerializeField] Transform _wireParentTransform;
    [SerializeField] Transform[] _buildingPoses;
    int posIndex = 0;
    public List<BuildingArea> _buildingsTemp;
    public GameObject PowerTower;

    int _buildingIndex => InventoryManager.instance.buildingIndex;
    int _wireIndex => InventoryManager.instance.wireIndex;

    private void Start()
    {
        TutorialSystem.instance.targets.Add(PowerTower);
        for (int i = 0; i < _buildings.Length; i++)
        {
            _buildings[i].GetComponent<BuildingController>().SetGrayColor();
            createBuilding(i + 1);
        }

        for (int i = 0; i < _wireIndex + 1; i++)
        {
            initCreateWire(i + 1);
        }
    }

    public int getAreaPrice(BuildingType buildingId)
    {
        return _productAreaSO.priceByType(buildingId);
    }

    public void onNewBuildingOpenEvent(int index)
    {
        InventoryManager.instance.buildingIndexUpgrade();
        openBuildingMesh(index, true);
        InventoryManager.instance.wireIndexUpgrade();
    }

    void openBuildingMesh(int index, bool withAnim = false)
    {
        BuildingArea product = _buildingsTemp[index];
        BuildingArea prevProduct = _buildingsTemp[index - 1];
        product.GetComponent<BuildingController>().SetGrayColor();
        product.onSold(true);
        if (index <= 4)
        {
            product.transform.position = _buildingPoses[index].position;
        }
        else
        {
            product.transform.position = _buildingPoses[index].position;
        }

        if (withAnim)
        {
            product.transform.position += Vector3.up * 3;
            product.transform.DOMoveY(0, .3f).SetEase(Ease.InBack).OnComplete(() =>
                ObjectCreator.instance.BuildingVFX(product.transform.position));
        }

        createNewWire(_wireIndex, prevProduct.gameObject);
    }

    void createNewWire(int index, GameObject buildingPrefab)
    {
        ObiSolver product = Instantiate(_wires[index]);
        product.transform.SetParent(buildingPrefab.GetComponentInChildren<ElectricPlug>().plugTransform);
        product.transform.localPosition = Vector3.zero;
        StartCoroutine(WaitForWireOptimization(product));
    }

    public void TutorialInitWire(int index)
    {
        if ((index - 1) >= _wires.Length)
            return;
        ObiSolver product = Instantiate(_wires[index - 1]);
        product.transform.SetParent(_wireParentTransform);
        product.transform.localPosition = Vector3.zero;
        StartCoroutine(WaitForWireOptimization(product));
    }

    void initCreateWire(int index)
    {
        if (InventoryManager.instance._inventorySO.tutorialIsDone)
        {
            if ((index - 1) >= _wires.Length)
                return;
            ObiSolver product = Instantiate(_wires[index - 1]);

            if (index - 1 == 0)
            {
                product.transform.SetParent(_wireParentTransform);
                product.transform.localPosition = Vector3.zero;
                product.GetComponentInChildren<ElectricWireItem>().onRelease(_buildingsTemp[index - 1]
                    .GetComponentInChildren<ElectricPlug>().plugTransform);
                _buildingsTemp[index - 1].GetComponentInChildren<ElectricPlug>().SetElectricty();
            }
            else if (index <= _wireIndex + 1)
            {
                product.transform.SetParent(_buildingsTemp[index - 2].transform.GetComponentInChildren<ElectricPlug>()
                    .plugTransform);
                product.transform.localPosition = Vector3.zero;
                product.GetComponentInChildren<ElectricWireItem>().onRelease(_buildingsTemp[index - 1]
                    .GetComponentInChildren<ElectricPlug>().plugTransform);
                _buildingsTemp[index - 1].GetComponentInChildren<ElectricPlug>().SetElectricty();
            }

            StartCoroutine(WaitForWireOptimization(product));
        }
    }

    IEnumerator WaitForWireOptimization(ObiSolver product)
    {
        yield return new WaitForSeconds(0.5f);
        product.simulateWhenInvisible = false;
    }

    void createBuilding(int index, bool withAnim = false)
    {
        if ((index - 1) >= _buildings.Length)
            return;

        BuildingArea product = Instantiate(_buildings[index - 1]);
        _buildingsTemp.Add(product);
        if (index - 1 == 0)
        {
            product.onSold(true);

            product.GetComponent<BuildingController>().SetTargetColor();

            if (!InventoryManager.instance._inventorySO.tutorialIsDone)
            {
                DOVirtual.DelayedCall(0.1f,
                    () =>
                    {
                        TutorialSystem.instance.targets.Add(product.GetComponentInChildren<ElectricPlug>().plugTransform
                            .gameObject);
                    });
            }
        }
        else if (index <= _buildingIndex + 1)
        {
            product.onSold(true);
            product.GetComponent<BuildingController>().SetTargetColor();
        }

        else product.onSold(false);


        if (index <= 4)
        {
            product.transform.position = _buildingPoses[posIndex].position;
        }
        else
        {
            product.transform.position = _buildingPoses[posIndex].position;
        }

        posIndex++;
        if (posIndex == _buildingPoses.Length) posIndex = 0;

        if (withAnim)
        {
            product.transform.position += Vector3.up * 3;
            product.transform.DOMoveY(0, .3f).SetEase(Ease.InBack).OnComplete(() =>
                ObjectCreator.instance.BuildingVFX(product.transform.position));
        }
    }
}