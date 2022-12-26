using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPlug : MonoBehaviour
{
    public Transform plugTransform;
    [SerializeField] float distanceToCollect;
    [HideInInspector] public bool IsConnected;
    bool workOneTime = false;
    [HideInInspector] public HosePump _hosePump;
    private void Update()
    {
        if (!IsConnected && GameManager.instance.player.WireOnHand)
        {
            float distance = (GameManager.instance.player.transform.position - transform.position).magnitude;

            if (distance < distanceToCollect)
            {
                SetElectricty();
                GameManager.instance.player.releaseMe(plugTransform);
            }
        }

    }
    public void SetElectricty()
    {
        if (!workOneTime)
        {
            workOneTime = true;
            if (!InventoryManager.instance._inventorySO.tutorialIsDone)
            {
                TutorialSystem.instance.UpgradeTargetIndex();
                TutorialSystem.instance.UpgradeTextIndex();
            }
        }
        IsConnected = true;        
        GetComponentInParent<BuildingController>().OnElectricConnected();
    }
}
