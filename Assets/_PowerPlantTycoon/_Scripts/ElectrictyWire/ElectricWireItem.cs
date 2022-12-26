using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElectricWireItem : CollectableItem
{
    bool workOneTime = false;
    [SerializeField] private HosePump _hosePump;
    [SerializeField] private GameObject _electricVFX;
    private bool canPump;
    private Color32 _baseWireColor;
    private GameObject _vfx;

    protected override void Start()
    {
        canCollect = true;
        canPump = false;
        transform.localPosition = new Vector3(-1.5f, -4f, 0.78f);
        _hosePump.enabled = false;

        _baseWireColor = new Color32(255, 165, 0, 255);
    }

    private void Update()
    {
        if (InventoryManager.instance.factoryEnergy < 0.01f && canPump)
        {
            _hosePump.enabled = false;
            _hosePump.GetComponent<Renderer>().material.color = Color.gray;
            if (_vfx != null)
                _vfx.SetActive(false);
        }

        if (InventoryManager.instance.factoryEnergy > 0.01f && canPump)
        {
            _hosePump.enabled = true;
            _hosePump.GetComponent<Renderer>().material.color = _baseWireColor;
            if (_vfx != null)
                _vfx.SetActive(true);
        }

        if (!collected && canCollect && !GameManager.instance.player.WireOnHand)
        {
            float distance = (GameManager.instance.player.transform.position - transform.position).magnitude;

            if (distance < distanceToCollect)
            {
                GameManager.instance.player.collectMe(this, StackingType.Magnet);
            }
        }
    }

    public override void onCollected(Vector3 targetPoint, Transform parent = null, float finalScaleFactor = 1,
        Action onCompleted = null)
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

        GameManager.instance.player.SetCurrentWire(this);
        GameManager.instance.player.WireOnHand = true;
        collected = true;
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0.2f, 0.238f);
        canCollect = false;
        GameManager.instance.player._fsm.switchState((int)CharacterState.CarryWire);
    }


    public override void onRelease(Transform parent = null)
    {
        //base.onRelease(parent);
        canPump = true;
        _hosePump.enabled = true;
        GameManager.instance.player.WireOnHand = false;
        DOVirtual.DelayedCall(0.1f, () => canCollect = false);
        EventManager.OnElectricSocketAreaExit();
        transform.SetParent(parent);
        DOVirtual.DelayedCall(0.1f, () => transform.localPosition = new Vector3(0, 0f, 0f)).OnComplete((() =>
        {
            _vfx = Instantiate(_electricVFX);
            _vfx.transform.SetParent(transform);
            _vfx.transform.localPosition = Vector3.zero;
            
            _vfx.SetActive(false);
        }));
        GameManager.instance.player.SetCurrentWire(null);
        
    }
}