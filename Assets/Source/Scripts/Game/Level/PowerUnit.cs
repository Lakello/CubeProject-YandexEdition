using CubeProject.Player;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(UseObject))]
	[RequireComponent(typeof(ChargeHolder))]
	public class PowerUnit : MonoBehaviour
	{
		[SerializeField] private bool _canGiveCharge;
		[SerializeField] private bool _canGetCharge;

		private UseObject _useObject;
		private ChargeHolder _selfChargeHolder;

		public ChargeHolder ChargeHolder => _selfChargeHolder ??= GetChargeHolder();

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _useObject);

			if (_selfChargeHolder is null)
			{
				gameObject.GetComponentElseThrow(out _selfChargeHolder);
			}
		}

		private void OnEnable() =>
			_useObject.TryUsing += OnTryUsing;

		private void OnDisable() =>
			_useObject.TryUsing -= OnTryUsing;

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