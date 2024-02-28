using System;
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

		public ChargeHolder ChargeHolder => _selfChargeHolder ??= GetChargeHolder();

		private void Awake()
		{
			if (_selfChargeHolder is null)
			{
				gameObject.GetComponentElseThrow(out _selfChargeHolder);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Cube cube))
			{
				OnTryUsing(cube);
			}
		}

		private ChargeHolder GetChargeHolder()
		{
			gameObject.GetComponentElseThrow(out _selfChargeHolder);

			return _selfChargeHolder;
		}

		private void OnTryUsing(Cube cube)
		{
			if (_selfChargeHolder.IsCharged && _canGiveCharge)
			{
				_selfChargeHolder.GivePowerTo(cube.ComponentsHolder.ChargeHolder);
			}
			else if (_canGetCharge)
			{
				cube.ComponentsHolder.ChargeHolder.GivePowerTo(_selfChargeHolder);
			}
		}
	}
}