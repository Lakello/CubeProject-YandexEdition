using System;
using CubeProject.PlayableCube;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	public class CubeStateHandler : MonoBehaviour
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