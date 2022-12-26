using Artera.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFSM : FSM
{
	protected override void onInitialize()
	{
		_states = new List<IState> {
			new CharacterIdleState((int)CharacterState.Idle, this),
			new CharacterWalkState((int)CharacterState.Walk, this),
			new CharacterMiningState((int)CharacterState.Mining, this),
			new CharacterCarryWireState((int)CharacterState.CarryWire, this)
		};
	}
}

public enum CharacterState
{
	Idle = 0,
	Walk,
	Mining,
	CarryWire

}
