using System;
using CubeProject.Game.Player.CubeService;
using CubeProject.Game.Player.FSM.States;
using LeadTools.Extensions;
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