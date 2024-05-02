using System;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using UnityEngine;

namespace Source.Scripts.LeadTools.Other.ColorChangers
{
	public class TriggerColorChangeBehaviour : ColorChangeBehaviour
	{
		[SerializeField] private float _enterDelay = 0.2f;
		[SerializeField] private float _exitDelay = 0.5f;

		private void Awake() =>
			Do(changer => changer.ChangeColor(false));

		private void OnTriggerEnter(Collider other) =>
			HandleTrigger(other, true);

		private void OnTriggerExit(Collider other) =>
			HandleTrigger(other, false);

		private void HandleTrigger(Collider other, bool isChanged)
		{
			if (other.TryGetComponent(out Cube _))
			{
				var delay = isChanged ? _enterDelay : _exitDelay;

				this.WaitTime(
					delay,
					() => Do(changer => changer.ChangeColor(isChanged)));
			}
		}
	}
}