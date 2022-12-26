using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private Text _text;
    private Tween _scaleTween;
    private Tween _positionTween;

    public void setText(int number)
    {
        _text.text = number + "$";
    }

    private void Start()
    {
        _scaleTween = transform.DOScale(transform.localScale.x * .3f, 1).SetDelay(.3f);
        _positionTween = transform.DOLocalMoveY(transform.position.y + 2, 1).OnComplete(() =>
        {
            _scaleTween.Kill();
            _positionTween.Kill();
            Destroy(gameObject);
        });
    }
}