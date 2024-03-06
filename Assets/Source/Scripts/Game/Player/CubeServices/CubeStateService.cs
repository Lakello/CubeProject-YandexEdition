using System;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeStateService : MonoBehaviour
	{
		[SerializeField] private CubeState _currentState = CubeState.Normal;

		public event Action<CubeState> StateChanged;

		public CubeState CurrentState => _currentState;

		public void EnterIn(CubeState state)
		{
			_currentState = state;
			StateChanged?.Invoke(_currentState);
		}
	}
}