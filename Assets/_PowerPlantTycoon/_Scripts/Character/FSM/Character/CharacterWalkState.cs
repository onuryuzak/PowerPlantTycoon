using UnityEngine;
using Artera.AI;

public class CharacterWalkState : State
{
    float _verticalAxis;
    float _horizontalAxis;
    private Character _character;
    private Animator _animator;

    public CharacterWalkState(int stateId, IFSM owner) : base(stateId, owner)
    {
        _character = owner.getComponent<Character>();
        _animator = _character.GetComponentInChildren<Animator>();
    }

    public override void onEnter()
    {
        base.onEnter();
        Debug.Log($"[CharacterWalkState::onEnter]");
        _animator.SetTrigger(Animations.Run);
    }

    public override void onExit()
    {
        base.onExit();
        _animator.ResetTrigger(Animations.Run);
    }

    public override void onUpdate()
    {
        base.onUpdate();

        if (_character.anyMovement)
        {
            _verticalAxis = _character.joystick.Vertical;
            _horizontalAxis = _character.joystick.Horizontal;
            Vector3 lookDir = new Vector3(_horizontalAxis, 0, _verticalAxis);

            _character.characterController.Move(lookDir * Time.deltaTime * _character.playerSpeed);
            _character.transform.rotation = Quaternion.Lerp(_character.transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * _character.rotationSpeed);

        }
        else
        {
            _character._fsm.switchState((int)CharacterState.Idle);
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