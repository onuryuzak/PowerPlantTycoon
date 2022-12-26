using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artera.AI;

public class CharacterMiningState : State
{
    float _verticalAxis;
    float _horizontalAxis;
    private Character _character;
    private Animator _animator;

    public CharacterMiningState(int stateId, IFSM owner) : base(stateId, owner)
    {
        _character = owner.getComponent<Character>();
        _animator = _character.GetComponentInChildren<Animator>();
    }

    public override void onEnter()
    {
        base.onEnter();
        Debug.Log($"[CharacterMiningAI::onEnter]");
        if (!_character.anyMovement)
        {
            _character.MagnetModelHolder.SetActive(false);
            _character.PickAxeModelHolder.SetActive(true);
            _animator.Play(Animations.Mining);
        }

        EventManager.MiningAreaExit += onMiningAreaExit;
    }

    public override void onExit()
    {
        base.onExit();
        EventManager.MiningAreaExit -= onMiningAreaExit;
    }


    void onMiningAreaExit()
    {
        _character.inMiningArea = false;
        _character.PickAxeModelHolder.SetActive(false);
        _character.MagnetModelHolder.SetActive(true);
        _character._fsm.switchState((int)CharacterState.Idle);
    }

    public override void onUpdate()
    {
        base.onUpdate();
        
        if (_character.anyMovement && _character.inMiningArea)
        {
            _character.MagnetModelHolder.SetActive(true);
            _character.PickAxeModelHolder.SetActive(false);
            _verticalAxis = _character.joystick.Vertical;
            _horizontalAxis = _character.joystick.Horizontal;
            Vector3 lookDir = new Vector3(_horizontalAxis, 0, _verticalAxis);
            _animator.SetTrigger(Animations.Run);

            _character.characterController.Move(lookDir * Time.deltaTime * _character.playerSpeed);
            _character.transform.rotation = Quaternion.Lerp(_character.transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * _character.rotationSpeed);

        }
        else
        {
            _character.MagnetModelHolder.SetActive(false);
            _character.PickAxeModelHolder.SetActive(true);
            _animator.Play(Animations.Mining);
        }
    }

    public override void onLateUpdate()
    {
        base.onLateUpdate();

        Vector3 pos = _character.characterController.transform.position;
        pos.y = 0f;

        _character.characterController.transform.position = pos;
    }
}
