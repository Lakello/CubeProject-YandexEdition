using System;
using LeadTools.Extensions;
using CubeProject.Game.PlayerStateMachine.States;
using UnityEngine;

namespace CubeProject.Game.Player
{
	[RequireComponent(typeof(CubeComponent))]
	public class CubeEntity : MonoBehaviour
	{
		private CubeComponent _component;

		public event Action Died;

		public CubeComponent Component => _component;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _component);

		public void Kill()
		{
			_component.StateMachine.EnterIn<DieState>();
			Died?.Invoke();
		}
	}
}