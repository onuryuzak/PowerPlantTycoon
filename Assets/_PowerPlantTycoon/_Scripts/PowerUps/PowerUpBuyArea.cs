using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PowerUpBuyArea : MonoBehaviour
{
    [SerializeField] PowerUpIdType _powerUpTypeId;
    [SerializeField] int _paySteps = 25;
    [SerializeField] TextMeshProUGUI _priceTextMesh;
    [SerializeField] TextMeshProUGUI _upgradeTextMesh;
    public bool _forcedMoney = false;
    public int _powerUpPrice = 1800;

    float _lastGetMoneyTime = 0;
    float _getMoneyFrequency = .05f;
    bool _complete = false;
    int _payAmountPerFrequency;
    int _index;
    bool waitingDone;
    Tween countDownTween;

    private void Start()
    {
        if (!_forcedMoney)
            _powerUpPrice = PowerUpManager.instance.getAreaPrice(_powerUpTypeId);
        _priceTextMesh.text = _powerUpPrice.ToString() + "$";
        _payAmountPerFrequency = (int)((float)_powerUpPrice / _paySteps);
        _index = _powerUpPrice / PowerUpManager.instance._playerPowerUpSO.priceIncreaseValue;
        if(_powerUpTypeId == PowerUpIdType.Speed)
        {
            _upgradeTextMesh.text = "+" + PowerUpManager.instance.playerSpeedIncreaseValue.ToString();
        }
        if (_powerUpTypeId == PowerUpIdType.CoalPiece)
        {
            _upgradeTextMesh.text = "+" + PowerUpManager.instance.playerCoalMiningCountIncreaseValue.ToString();
        }

    }
    public void SetCountdownFill()
    {
        countDownTween = GameManager.instance.player.radialCountdown.DOFillAmount(0, 1.5f)
            .OnComplete(() => waitingDone = true); ;
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
        if (other.CompareTag("Player") && (Time.time - _lastGetMoneyTime) > _getMoneyFrequency && InventoryManager.instance.money >= _payAmountPerFrequency && !_complete && waitingDone)
        {
            _lastGetMoneyTime = Time.time;
            _powerUpPrice -= _payAmountPerFrequency;
            InventoryManager.instance.addMoney(-_payAmountPerFrequency);

            Vector2 rnd = Random.insideUnitCircle / 4;
            GameManager.instance.player.spendMoneyEffect(transform.position + new Vector3(rnd.x, 0, rnd.y) * 4, 1, (money) =>
            {
                Destroy(money.gameObject);

                if (_powerUpPrice <= 0 && !_complete)
                {
                    _complete = true;
                    onComplete();
                    _powerUpPrice = PowerUpManager.instance.getAreaPrice(_powerUpTypeId);
                    _priceTextMesh.text = _powerUpPrice.ToString() + "$";
                    if (_powerUpTypeId == PowerUpIdType.Speed)
                    {
                        _upgradeTextMesh.text = "+" + PowerUpManager.instance.playerSpeedIncreaseValue.ToString();
                    }
                    if (_powerUpTypeId == PowerUpIdType.CoalPiece)
                    {
                        _upgradeTextMesh.text = "+" + PowerUpManager.instance.playerCoalMiningCountIncreaseValue.ToString();
                    }
                }
            });

            _priceTextMesh.text = _powerUpPrice.ToString() + "$";
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
        PowerUpManager.instance.onPowerUpUpgrade(_powerUpTypeId);
        _complete = false;
    }
}


