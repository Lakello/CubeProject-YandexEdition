using System;
using System.Linq;
using UnityEngine;

namespace CubeProject.Game
{
	public abstract class ChargeConsumer : MonoBehaviour, IChargeable
	{
		[SerializeField] private bool _needAllCharge;
		[SerializeField] private PowerUnit[] _powers;
		[SerializeField] private bool _isAlwaysCharged;

		public event Action ChargeChanged;

		public bool IsCharged => _isAlwaysCharged || _needAllCharge ? CheckAllCharge() : CheckAnyCharge();

		private void Start()
		{
			if (_isAlwaysCharged)
			{
				OnChargeChanged();
			}
		}

		protected virtual void OnEnable() =>
			Subscribe(OnChargeChanged);

		protected virtual void OnDisable() =>
			Unsubscribe(OnChargeChanged);

		private void OnChargeChanged() =>
			ChargeChanged?.Invoke();

		private bool CheckAnyCharge() =>
			_powers.Any(holder => holder.ChargeHolder.IsCharged);

		private bool CheckAllCharge() =>
			_powers.All(holder => holder.ChargeHolder.IsCharged);
		
		private void Subscribe(Action observer)
		{
			if (_powers is null)
			{
				return;
			}

			foreach (var holder in _powers)
			{
				holder.ChargeHolder.ChargeChanged += observer;
			}
		}

		private void Unsubscribe(Action observer)
		{
			if (_powers is null)
			{
				return;
			}

			foreach (var holder in _powers)
			{
				holder.ChargeHolder.ChargeChanged -= observer;
			}
		}
	}
}