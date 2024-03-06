using System.Runtime.CompilerServices;
using CubeProject.PlayableCube;
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
			var cubeIsCharged = cube.ServiceHolder.ChargeHolder.IsCharged;
			var selfIsCharged = _selfChargeHolder.IsCharged;

			if (selfIsCharged && cubeIsCharged)
			{
				return;
			}
			
			if (selfIsCharged && _canGiveCharge)
			{
				_selfChargeHolder.GivePowerTo(cube.ServiceHolder.ChargeHolder);
			}
			else if (cubeIsCharged && _canGetCharge)
			{
				cube.ServiceHolder.ChargeHolder.GivePowerTo(_selfChargeHolder);
			}
		}
	}
}