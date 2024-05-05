using System;
using LeadTools.StateMachine;
using Source.Scripts.Game.tateMachine;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public interface IInputService
	{
		public event Action<Vector3> Moving;
		public event Action MenuKeyChanged;

		public void Init(PlayerInput playerInput, IStateChangeable<CubeStateMachine> cubeStateMachine);
	}
}