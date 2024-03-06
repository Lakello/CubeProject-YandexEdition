using System;
using LeadTools.Extensions;
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
			Died?.Invoke();
			_serviceHolder.StateHandler.EnterIn(CubeState.Dying);
		}
	}
}