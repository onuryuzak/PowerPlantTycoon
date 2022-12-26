using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera;

[CreateAssetMenu(fileName = "Inventory", menuName = "Artera/Inventory")]
public class InventorySO : BaseScriptableObject
{
    public int money;
    public int buildingIndex;
    public int wireIndex;
    public float factoryEnergy;
    public float factoryFillAmount;
    public bool tutorialIsDone;

    public override void reset()
    {
        money = 0;
        buildingIndex = 0;
        factoryEnergy = 0;
        factoryFillAmount = 0f;
        wireIndex = 0;
        tutorialIsDone = false;
    }
}
