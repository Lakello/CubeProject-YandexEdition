using System;
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
		private const string Clip = "_Clip";

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

			OnChargeChanged();
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
			var data = GetStateData(_chargeConsumer.IsCharged);
			
			_maskChangeCoroutine = this.PlaySmoothChangeValue(
				(currentTime) =>
				{
					LerpColor(
						data.Gradient,
						currentTime);

					LerpCurve(data.MaskPowerCurve, currentTime, SetMask);
					LerpCurve(data.ClipCurve, currentTime, SetClip);
				},
				data.ChangingStateDuration);

			yield return _maskChangeCoroutine;
		}

		private void LerpCurve(AnimationCurve curve, float currentTime, Action<float> set)
		{
			var result = curve.Evaluate(currentTime);

			set(result);
		}

		private void LerpColor(Gradient gradient, float currentTime)
		{
			var resultMaskColor = gradient.Evaluate(currentTime);
			
			SetColor(resultMaskColor);
		}

		private BarrierViewData GetStateData(bool isCharged) =>
			isCharged is false ? _openData : _closeData;

		private void SetColor(Color color) =>
			_meshRendererWall.material.SetColor(BaseColor, color);

		private void SetClip(float clip) =>
			_meshRendererWall.material.SetFloat(Clip, clip);

		private void SetMask(float maskPower) =>
			_meshRendererWall.material.SetFloat(PowerValue, maskPower);
	}
}