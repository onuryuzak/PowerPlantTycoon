using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoneyBox : MonoBehaviour
{
    [SerializeField] Text _moneyText;
    [SerializeField] Image _moneyImage;

    [Header("Events")]
    public MoneyChangeEventSO onMoneyChangeEvent;

    Tweener _moneyTweener;

    private void Start()
    {
        _moneyText.text = InventoryManager.instance.money.ToString();
    }

    void onMoneyChange(int money)
    {
        _moneyText.text = money.ToString();

        if (_moneyTweener != null)
        {
            _moneyTweener.Kill(true);
            _moneyTweener = null;
        }

        _moneyTweener = _moneyImage.transform.DOScale(0.25f, .1f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            _moneyImage.transform.localScale = Vector3.one * 0.2f;
        });
    }

    private void OnEnable()
    {
        onMoneyChangeEvent.onMoneyChangeListener.AddListener(onMoneyChange);
    }

    private void OnDisable()
    {
        onMoneyChangeEvent.onMoneyChangeListener.RemoveListener(onMoneyChange);
    }
}