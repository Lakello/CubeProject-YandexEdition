using System;
using CubeProject.Player;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public interface IInputHandler
	{
		public event Action<Vector3> MoveKeyChanged;

		public event Action UsePressed;

		public event Action UseReleased;

		public event Action MenuKeyChanged;

		public void Init(PlayerInput playerInput, CubeStateHandler stateHandler);
	}
}