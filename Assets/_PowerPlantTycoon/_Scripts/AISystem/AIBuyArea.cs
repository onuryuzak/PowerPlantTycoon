using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AIBuyArea : MonoBehaviour
{
    [SerializeField] AIType _aiTypeId;
    [SerializeField] int _paySteps = 25;
    [SerializeField] TextMeshProUGUI _priceTextMesh;
    public bool _forcedMoney = false;
    public int _aiOpenPrice = 1800;

    float _lastGetMoneyTime = 0;
    float _getMoneyFrequency = .05f;
    bool _complete = false;
    int _payAmountPerFrequency;
    private bool _isBought;
    bool waitingDone;
    Tween countDownTween;

    private void Start()
    {
        if (!_forcedMoney)
            _aiOpenPrice = AIManager.instance.getAreaPrice(_aiTypeId);
        _priceTextMesh.text = _aiOpenPrice.ToString() + "$";
        _payAmountPerFrequency = (int)((float)_aiOpenPrice / _paySteps);
        _isBought = AIManager.instance.AICheckSO.getStateByType(_aiTypeId);
        if (_isBought)
        {
            _isBought = false;
            AIManager.instance.CreateAI(_aiTypeId, transform.position);
            gameObject.SetActive(false);
        }
    }

    public void SetCountdownFill()
    {
        countDownTween = GameManager.instance.player.radialCountdown.DOFillAmount(0, 1.5f)
            .OnComplete(() => waitingDone = true);
        ;
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
            _aiOpenPrice -= _payAmountPerFrequency;
            InventoryManager.instance.addMoney(-_payAmountPerFrequency);

            Vector2 rnd = Random.insideUnitCircle / 4;
            GameManager.instance.player.spendMoneyEffect(transform.position + new Vector3(rnd.x, 0, rnd.y) * 4, 1,
                (money) =>
                {
                    Destroy(money.gameObject);

                    if (_aiOpenPrice <= 0 && !_complete)
                    {
                        _complete = true;

                        _priceTextMesh.gameObject.SetActive(false);
                        GameManager.instance.player.SetRadialCountDownActive(false);
                        gameObject.SetActive(false);
                        onComplete();
                    }
                });

            _priceTextMesh.text = _aiOpenPrice.ToString() + "$";
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
        AIManager.instance.CreateAI(_aiTypeId, transform.position);
        AIManager.instance.AICheckSO.SetStateByType(_aiTypeId, true);
        Database.instance.saveGame();
    }
}