using System;
using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;

namespace CubeProject.Game
{
	public class ChargeView : MonoBehaviour
	{
		[SerializeField] private HDRColorChanger _hdrColorChanger;

		private IChargeable _chargeable;

		public event Action<bool> ChargeViewChanged;

		protected IChargeable Chargeable => _chargeable;

		protected HDRColorChanger ColorChanger => _hdrColorChanger;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeable);

			_hdrColorChanger.Init(() => _chargeable.IsCharged);
		}

		private void OnEnable()
		{
			_chargeable.ChargeChanged += OnChargeChanged;

			OnChargeChanged();
		}

		private void OnDisable() =>
			_chargeable.ChargeChanged -= OnChargeChanged;

		protected void CallChanged() =>
			ChargeViewChanged?.Invoke(_chargeable.IsCharged);

		protected virtual void OnChargeChanged()
		{
			_hdrColorChanger.ChangeColor();

			CallChanged();
		}
	}
}