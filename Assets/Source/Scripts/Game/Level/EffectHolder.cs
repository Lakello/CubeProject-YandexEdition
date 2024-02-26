using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeView))]
	public abstract class EffectHolder : MonoBehaviour
	{
		private ChargeView _chargeView;

		private void Awake() =>
			gameObject.GetComponentElseThrow(out _chargeView);

		private void OnEnable() =>
			_chargeView.ChargeViewChanged += OnChargeViewChanged;

		private void OnDisable() =>
			_chargeView.ChargeViewChanged -= OnChargeViewChanged;

		private void OnChargeViewChanged(bool isCharged)
		{
			if (isCharged)
			{
				Play();
			}
			else
			{
				Stop();
			}
		}

		protected abstract void Play();

		protected abstract void Stop();
	}
}