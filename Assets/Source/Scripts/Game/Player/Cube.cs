using System;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.PlayableCube
{
	[RequireComponent(typeof(CubeComponentsHolder))]
	public class Cube : MonoBehaviour
	{
		private CubeComponentsHolder _componentsHolder;

		public event Action Died;

		public CubeComponentsHolder ComponentsHolder => _componentsHolder;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _componentsHolder);

		public void Kill()
		{
			Died?.Invoke();
			_componentsHolder.StateHandler.EnterIn(CubeState.Dying);
		}
	}
}