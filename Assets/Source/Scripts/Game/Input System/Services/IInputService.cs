using System;
using CubeProject.PlayableCube;
using UnityEngine;

namespace CubeProject.InputSystem
{
	public interface IInputService
	{
		public event Action<Vector3> Moving;

		public event Action MenuKeyChanged;

		public void Init(PlayerInput playerInput, CubeStateHandler stateHandler);
	}
}