using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BuildingBuyArea : MonoBehaviour
{
    [SerializeField] BuildingType _willBeOpenBuildingTypeId;
    [SerializeField] int _paySteps = 25;
    [SerializeField] TextMeshPro _textMesh;
    public bool _forcedMoney = false;
    public int _newAreaProductPrice = 1800;

    float _lastGetMoneyTime = 0;
    float _getMoneyFrequency = .05f;
    bool _complete = false;
    int _payAmountPerFrequency;
    bool waitingDone;
    Tween countDownTween;

    private void Start()
    {
        if (!_forcedMoney)
            _newAreaProductPrice = BuildingManager.instance.getAreaPrice(_willBeOpenBuildingTypeId);
        _textMesh.text = _newAreaProductPrice.ToString()+"$";
        _payAmountPerFrequency = (int)((float)_newAreaProductPrice / _paySteps);
    }

    public void SetCountdownFill()
    {
        countDownTween = GameManager.instance.player.radialCountdown.DOFillAmount(0, 1.5f)
            .OnComplete(() =>
            {
                waitingDone = true;
                countDownTween.Kill();
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.player.SetRadialCountDownActive(true);
            SetCountdownFill();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && (Time.time - _lastGetMoneyTime) > _getMoneyFrequency &&
            InventoryManager.instance.money >= _payAmountPerFrequency && !_complete && waitingDone)
        {
            GameManager.instance.player.SetRadialCountDownActive(true);
            _lastGetMoneyTime = Time.time;
            _newAreaProductPrice -= _payAmountPerFrequency;
            InventoryManager.instance.addMoney(-_payAmountPerFrequency);

            Vector2 rnd = Random.insideUnitCircle / 4;
            GameManager.instance.player.spendMoneyEffect(transform.position + new Vector3(rnd.x, 0, rnd.y) * 4, 1,
                (money) =>
                {
                    Destroy(money.gameObject);

                    if (_newAreaProductPrice <= 0 && !_complete)
                    {
                        _complete = true;
                        _textMesh.gameObject.SetActive(false);
                        gameObject.SetActive(false);
                        GameManager.instance.player.radialCountdown.fillAmount = 1;
                        countDownTween.Kill();
                        GameManager.instance.player.SetRadialCountDownActive(false);
                        waitingDone = false;
                        DOVirtual.DelayedCall(.5f, () => { onComplete(); });
                    }
                });

            _textMesh.text = _newAreaProductPrice.ToString()+"$";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.player.radialCountdown.fillAmount = 1;
            countDownTween.Kill();
            GameManager.instance.player.SetRadialCountDownActive(false);
            waitingDone = false;
        }
    }

    void onComplete()
    {
        _newAreaProductPrice = 0;
        BuildingManager.instance.onNewBuildingOpenEvent((int)_willBeOpenBuildingTypeId);
    }
}