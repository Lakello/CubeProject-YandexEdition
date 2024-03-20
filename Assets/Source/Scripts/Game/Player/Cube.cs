using System;
using LeadTools.Extensions;
using Source.Scripts.Game.tateMachine.States;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeServiceHolder))]
	public class Cube : MonoBehaviour
	{
		private CubeServiceHolder _serviceHolder;

		public event Action Died;

		public CubeServiceHolder ServiceHolder => _serviceHolder;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _serviceHolder);

		public void Kill()
		{
			_serviceHolder.StateMachine.EnterIn<DieState>();
			Died?.Invoke();
		}
	}
}