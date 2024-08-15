using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
	public sealed class ChargeConsumer : MonoBehaviour, IChargeable
	{
		[SerializeField] private bool _isAlwaysCharged;
		[SerializeField] [HideIf(nameof(_isAlwaysCharged))] private bool _needAllCharge;
		[SerializeField] [HideIf(nameof(_isAlwaysCharged))] private GameObject[] _gameObjectsOnChargeable;

		private IChargeable[] _chargeables;
		private bool _isCharged;

		public event Action ChargeChanged;

		public bool IsCharged => _isAlwaysCharged || _isCharged;

		private void OnValidate()
		{
			if (_gameObjectsOnChargeable is { Length: < 1 })
				return;

			for (int i = 0; i < _gameObjectsOnChargeable.Length; i++)
			{
				if (_gameObjectsOnChargeable[i].TryGetComponent(out IChargeable _) is false)
				{
					_gameObjectsOnChargeable[i] = null;

					Debug.LogError($"{nameof(_gameObjectsOnChargeable)} Required {nameof(IChargeable)}", gameObject);
				}
			}
		}

		private void Awake()
		{
			_chargeables = new IChargeable[_gameObjectsOnChargeable.Length];

			for (int i = 0; i < _gameObjectsOnChargeable.Length; i++)
			{
				if (_gameObjectsOnChargeable[i] == null)
					continue;

				if (_gameObjectsOnChargeable[i].TryGetComponent(out IChargeable chargeable))
					_chargeables[i] = chargeable;
			}
		}

		private void Start()
		{
			if (_isAlwaysCharged)
				OnChargeChanged();
		}

		private void OnEnable()
		{
			if (_isAlwaysCharged is false)
				Subscribe();
		}

		private void OnDisable()
		{
			if (_isAlwaysCharged is false)
				Unsubscribe();
		}

		private bool CheckAnyCharge() =>
			_chargeables.Any(holder => holder.IsCharged);

		private bool CheckAllCharge() =>
			_chargeables.All(holder => holder.IsCharged);

		private void OnChargeChanged()
		{
			bool isCharged;

			if (_needAllCharge)
				isCharged = CheckAllCharge();
			else
				isCharged = CheckAnyCharge();

			if (isCharged == _isCharged)
				return;

			_isCharged = isCharged;

			ChargeChanged?.Invoke();
		}

		private void Subscribe()
		{
			foreach (var holder in _chargeables)
				holder.ChargeChanged += OnChargeChanged;
		}

		private void Unsubscribe()
		{
			foreach (var holder in _chargeables)
				holder.ChargeChanged -= OnChargeChanged;
		}
	}
}