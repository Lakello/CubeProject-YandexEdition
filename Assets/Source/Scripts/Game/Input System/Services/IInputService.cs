using System;
using UnityEngine;

namespace CubeProject.Game.InputSystem
{
	public interface IInputService : IDisposable
	{
		public event Action<Vector3> Moving;
		public event Action MenuKeyChanged;
	}
}