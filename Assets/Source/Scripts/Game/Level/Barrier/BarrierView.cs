using System;
using DG.Tweening;
using LeadTools.Extensions;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(ChargeConsumer))]
	public class BarrierView : MonoBehaviour
	{
		private const string PowerValue = "_PowerValue";
		private const string BaseColor = "_BaseColor";
		private const string Clip = "_Clip";

		[SerializeField] private BarrierViewData _animationData;
		[SerializeField] private MeshRenderer _meshRendererWall;

		private ChargeConsumer _chargeConsumer;
		private Tweener _animation;

		private void Awake()
		{
			gameObject.GetComponentElseThrow(out _chargeConsumer);

			_animation = DOTween
				.To(progress =>
					{
						LerpColor(_animationData.Gradient, progress);
						LerpCurve(_animationData.ClipCurve, progress, SetClip);
						LerpCurve(_animationData.MaskPowerCurve, progress, SetMask);
					},
					0,
					1,
					_animationData.ChangingStateDuration)
				.SetAutoKill(false)
				.Pause();

			OnChargeChanged();
		}

		private void OnEnable() =>
			_chargeConsumer.ChargeChanged += OnChargeChanged;

		private void OnDisable()
		{
			_animation.Kill();
			_chargeConsumer.ChargeChanged -= OnChargeChanged;
		}

		private void OnChargeChanged()
		{
			_animation.Pause();

			if (_chargeConsumer.IsCharged)
				_animation.PlayBackwards();
			else
				_animation.PlayForward();
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

		private void SetColor(Color color) =>
			_meshRendererWall.material.SetColor(BaseColor, color);

		private void SetClip(float clip) =>
			_meshRendererWall.material.SetFloat(Clip, clip);

		private void SetMask(float maskPower) =>
			_meshRendererWall.material.SetFloat(PowerValue, maskPower);
	}
}