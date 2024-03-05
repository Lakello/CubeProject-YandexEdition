using System.Collections;
using LeadTools.Extensions;
using UnityEngine;

namespace CubeProject.Game
{
	[RequireComponent(typeof(ChargeConsumer))]
	public class BarrierView : MonoBehaviour
	{
		private const string PowerValue = "_PowerValue";
		private const string BaseColor = "_BaseColor";

		[SerializeField] private BarrierViewData _openData;
		[SerializeField] private BarrierViewData _closeData;
		[SerializeField] private MeshRenderer _meshRendererWall;

		private Coroutine _maskChangeCoroutine;
		private Coroutine _changeMaskCoroutine;
		private Coroutine _changeStateCoroutine;
		private ChargeConsumer _chargeConsumer;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeConsumer);

			var data = GetStateData();

			SetColor(data.Color.CalculateIntensityColor(data.Intensity));
			SetMask(data.MaskPower);
		}

		private void OnEnable() =>
			_chargeConsumer.ChargeChanged += OnChargeChanged;

		private void OnDisable() =>
			_chargeConsumer.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged()
		{
			this.StopRoutine(_changeMaskCoroutine);
			this.StopRoutine(_maskChangeCoroutine);
			this.StopRoutine(_changeStateCoroutine);

			_changeStateCoroutine = StartCoroutine(ChangeState());
		}

		private IEnumerator ChangeState()
		{
			var data = GetStateData();

			_maskChangeCoroutine = this.PlaySmoothChangeValue(
				(currentTime) =>
				{
					LerpColor(
						GetColor(),
						data.Color.CalculateIntensityColor(data.Intensity),
						currentTime);

					LerpMask(data.AnimationMaskPower, currentTime);
				},
				data.ChangingStateDuration);

			yield return _maskChangeCoroutine;
		}

		private void LerpMask(AnimationCurve curve, float currentTime)
		{
			var resultMaskPower = curve.Evaluate(currentTime);

			SetMask(resultMaskPower);
		}

		private void LerpColor(Color currentColor, Color targetColor, float currentTime)
		{
			var resultMaskColor = Color.Lerp(currentColor, targetColor, currentTime);

			SetColor(resultMaskColor);
		}

		private BarrierViewData GetStateData() =>
			_chargeConsumer.IsCharged is false ? _openData : _closeData;

		private void SetColor(Color color) =>
			_meshRendererWall.material.SetColor(BaseColor, color);

		private Color GetColor() =>
			_meshRendererWall.material.GetColor(BaseColor);

		private void SetMask(float maskPower) =>
			_meshRendererWall.material.SetFloat(PowerValue, maskPower);
	}
}