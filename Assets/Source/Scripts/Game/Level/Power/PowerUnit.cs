using System.Runtime.CompilerServices;
using CubeProject.Player;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeHolder))]
	public class PowerUnit : MonoBehaviour
	{
		[SerializeField] private bool _canGiveCharge;
		[SerializeField] private bool _canGetCharge;

		private ChargeHolder _selfChargeHolder;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _selfChargeHolder);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube cube))
			{
				OnTryUsing(cube);
			}
		}

		private void OnTryUsing(Cube cube)
		{
			var cubeIsCharged = cube.ComponentsHolder.ChargeHolder.IsCharged;
			var selfIsCharged = _selfChargeHolder.IsCharged;

			if (selfIsCharged && cubeIsCharged)
			{
				return;
			}
			
			if (selfIsCharged && _canGiveCharge)
			{
				_selfChargeHolder.GivePowerTo(cube.ComponentsHolder.ChargeHolder);
			}
			else if (cubeIsCharged && _canGetCharge)
			{
				cube.ComponentsHolder.ChargeHolder.GivePowerTo(_selfChargeHolder);
			}
		}
	}
}