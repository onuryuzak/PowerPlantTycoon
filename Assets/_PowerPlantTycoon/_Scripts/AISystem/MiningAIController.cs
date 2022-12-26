using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiningAIController : MonoBehaviour
{
    private Animator _animator;
    private MiningAnimationEvent _event;
    private Transform _targetMovePosition;
    private Transform _coalPilePosition;
    private bool lookNow;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _event = GetComponentInChildren<MiningAnimationEvent>();
        GameManager.instance._miningAIController = this;
        StartMovement();
    }

    private void Update()
    {
        if (lookNow)
        {
            Quaternion lookOnLook =
       Quaternion.LookRotation(AIManager.instance._minerTargetPosition.position - transform.position);

            transform.rotation =
                   Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 5f);
        }
    }

    private void CreateInitAI()
    {
        MiningAIController _miningAIController = Instantiate(AIManager.instance.miningAIController);
        _miningAIController.transform.position = Vector3.zero;
    }
    public void SetInitPosition(Vector3 initPos)
    {
        transform.position = initPos;
    }

    private void StartMovement()
    {
        _event.GetPileOfCoalPosition(AIManager.instance._coalPosition);
        lookNow = true;
        transform.DOMove(AIManager.instance._minerTargetPosition.position, 4f).OnComplete(() =>
        {
            _animator.SetTrigger("Mining");
        });
    }
}
