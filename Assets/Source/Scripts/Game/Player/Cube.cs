using System;
using LeadTools.Extensions;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeComponent))]
	public class Cube : MonoBehaviour
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