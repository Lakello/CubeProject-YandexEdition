using CubeProject.Game.Level.Charge;
using CubeProject.Game.Player;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game.Level.Power
{
	[RequireComponent(typeof(ChargeHolder))]
	public class PowerUnit : MonoBehaviour
	{
		[SerializeField] private bool _canGiveCharge;
		[SerializeField] private bool _canGetCharge;

		private ChargeHolder _selfChargeHolder;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _selfChargeHolder);

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CubeEntity cube))
				OnTryUsing(cube);
		}

		private void OnTryUsing(CubeEntity cubeEntity)
		{
			var cubeIsCharged = cubeEntity.Component.ChargeHolder.IsCharged;
			var selfIsCharged = _selfChargeHolder.IsCharged;

			if (selfIsCharged && cubeIsCharged)
				return;

			if (selfIsCharged && _canGiveCharge)
				_selfChargeHolder.GivePowerTo(cubeEntity.Component.ChargeHolder);
			else if (cubeIsCharged && _canGetCharge)
				cubeEntity.Component.ChargeHolder.GivePowerTo(_selfChargeHolder);
		}
	}
}