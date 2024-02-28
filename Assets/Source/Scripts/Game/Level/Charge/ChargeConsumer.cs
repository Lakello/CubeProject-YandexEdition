using System;
using System.Linq;
using LeadTools.NaughtyAttributes;
using UnityEngine;

namespace CubeProject.Game
{
	public sealed class ChargeConsumer : MonoBehaviour, IChargeable
	{
		[SerializeField] private bool _isAlwaysCharged;
		[SerializeField] [ShowIf(nameof(IsShowNeedAllCharge))] private bool _needAllCharge;
		[SerializeField] [ShowIf(nameof(IsShowNeedAllCharge))] private GameObject[] _gameObjectsOnChargeable;

		private IChargeable[] _chargeables;

		public event Action ChargeChanged;

		public bool IsCharged => _isAlwaysCharged || _needAllCharge ? CheckAllCharge() : CheckAnyCharge();

		private bool IsShowNeedAllCharge => _isAlwaysCharged is false;

		private void OnValidate()
		{
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
				{
					continue;
				}

				if (_gameObjectsOnChargeable[i].TryGetComponent(out IChargeable chargeable))
				{
					_chargeables[i] = chargeable;
				}
			}
		}

		private void Start()
		{
			if (_isAlwaysCharged)
			{
				OnChargeChanged();
			}
		}

		private void OnEnable()
		{
			if (_isAlwaysCharged is false)
			{
				Subscribe();
			}
		}

		private void OnDisable()
		{
			if (_isAlwaysCharged is false)
			{
				Unsubscribe();
			}
		}

		private bool CheckAnyCharge() =>
			_chargeables.Any(holder => holder.IsCharged);

		private bool CheckAllCharge() =>
			_chargeables.All(holder => holder.IsCharged);

		private void OnChargeChanged() =>
			ChargeChanged?.Invoke();

		private void Subscribe()
		{
			foreach (var holder in _chargeables)
			{
				holder.ChargeChanged += OnChargeChanged;
			}
		}

		private void Unsubscribe()
		{
			foreach (var holder in _chargeables)
			{
				holder.ChargeChanged -= OnChargeChanged;
			}
		}
	}
}