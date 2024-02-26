using System;
using System.Collections;
using CubeProject.Game;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Tips
{
	[Serializable]
	public class UseTipKeyView : ChargeView
	{
		[SerializeField] private float _speedPulsation;

		private Coroutine _pulsationCoroutine;

		protected override void OnChargeChanged()
		{
			StopPulsation();

			if (((UseTipKeyHandler)Chargeable).CanUse is false)
			{
				DeActive();
			}
			else if (Chargeable.IsCharged)
			{
				Active();
			}
			else
			{
				_pulsationCoroutine = StartCoroutine(Pulsation());
			}
		}

		private void DeActive() =>
			ColorChanger.ChangeColor(false);

		private void Active() =>
			ColorChanger.ChangeColor(true);

		private void StopPulsation() =>
			this.StopRoutine(_pulsationCoroutine);

		private IEnumerator Pulsation()
		{
			var startIntensity = ColorChanger.IntensityDischargedColor;
			var targetIntensity = ColorChanger.IntensityChargedColor;
			var color = ColorChanger.ChargedColor;

			float intensity;

			while (enabled)
			{
				yield return this.PlaySmoothChangeValue(
					(currentTime) =>
					{
						intensity = Mathf.Lerp(startIntensity, targetIntensity, currentTime);
						ColorChanger.ChangeColor(color.CalculateIntensityColor(intensity));
					},
					_speedPulsation);

				(startIntensity, targetIntensity) = (targetIntensity, startIntensity);

				yield return null;
			}
		}
	}
}