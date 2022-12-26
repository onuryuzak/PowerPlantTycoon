using UnityEngine;
using Artera.AI;

public class CharacterIdleState : State
{
	private Character _character;
	private Animator _animator;

	public CharacterIdleState(int stateId, IFSM owner) : base(stateId, owner)
	{
		_character = owner.getComponent<Character>();
		_animator = _character.GetComponentInChildren<Animator>();
	}

	public override void onEnter()
	{
		base.onEnter();
		_animator.SetTrigger(Animations.Idle);
		_character.MagnetModelHolder.SetActive(true);
		Debug.Log($"[CharacterIdleState::onEnter]");
		
		
	}

	public override void onExit()
	{
		base.onExit();
		_animator.ResetTrigger(Animations.Idle);
	}

	public override void onUpdate()
	{
		base.onUpdate();
		_character.characterController.Move(Vector3.zero);
		if (_character.anyMovement)
		{
			_character._fsm.switchState((int)CharacterState.Walk);
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
