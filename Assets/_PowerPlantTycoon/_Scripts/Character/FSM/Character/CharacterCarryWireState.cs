using Artera.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCarryWireState : State
{
    float _verticalAxis;
    float _horizontalAxis;
    private Character _character;
    private Animator _animator;

    public CharacterCarryWireState(int stateId, IFSM owner) : base(stateId, owner)
    {
        _character = owner.getComponent<Character>();
        _animator = _character.GetComponentInChildren<Animator>();
    }
    public override void onEnter()
    {
        base.onEnter();
        _character.HoseHead.SetActive(true);
        _character.MagnetModelHolder.SetActive(false);
        _animator.SetTrigger(Animations.CarryRun);
        EventManager.ElectricSocketAreaExit += onElectricSocketAreaExit;
        Debug.Log($"[CharacterCarryWireState::onEnter]");

    }
    public override void onExit()
    {
        base.onExit();
        _animator.ResetTrigger(Animations.CarryRun);
        _character.HoseHead.SetActive(false);
        _character.MagnetModelHolder.SetActive(true);
        EventManager.ElectricSocketAreaExit -= onElectricSocketAreaExit;
    }

    private void onElectricSocketAreaExit()
    {
        _character.MagnetModelHolder.SetActive(true);
        _character._fsm.switchState((int)CharacterState.Walk);
    }

    public override void onUpdate()
    {
        base.onUpdate();
        if (_character.anyMovement)
        {
            _animator.SetTrigger(Animations.CarryRun);
            _verticalAxis = _character.joystick.Vertical;
            _horizontalAxis = _character.joystick.Horizontal;
            Vector3 lookDir = new Vector3(_horizontalAxis, 0, _verticalAxis);
            _character.characterController.Move(lookDir * Time.deltaTime * _character.playerSpeed);
            _character.transform.rotation = Quaternion.Lerp(_character.transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * _character.rotationSpeed);
        }
        else
        {
            _animator.SetTrigger(Animations.CarryIdle);
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
