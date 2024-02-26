using System;
using System.Collections;
using LeadTools.Extensions;
using LeadTools.Other;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CubeProject.Game
{
	public class BarrierView : MonoBehaviour
	{
		private const string MaskPower = "_MaskPower";
		private const string DotsColor = "_DotsColor";
		private const string BaseColor = "_BaseColor";
		private const string BorderColor = "_BorderColor";
		private const string IntersectionColor = "_IntersectionColor";

		[SerializeField] private FloatRange _rangeMaskPower;
		[SerializeField] private Color _onColor;
		[SerializeField] private Color _offColor;
		[SerializeField] [Range(-10f, 10f)] private float _intersectionOnIntensity = 0;
		[SerializeField] [Range(-10f, 10f)] private float _intersectionOffIntensity = -5;
		[SerializeField] [Range(-10f, 10f)] private float _intensityOnColor = 4;
		[SerializeField] [Range(-10f, 10f)] private float _intensityOffColor = 1;
		[SerializeField] [Range(0f, 20f)] private float _onMaskPower;
		[SerializeField] [Range(0f, 20f)] private float _offMaskPower;
		[SerializeField] private float _timeChangeMaskPowerOnIdle;
		[SerializeField] private float _timeChangeMaskPower;
		[SerializeField] private MeshRenderer _meshRendererWall;
		[SerializeField] private MeshRenderer[] _meshRenderersBase;
		[SerializeField] private Barrier _barrier;

		private Coroutine _maskStateCoroutine;
		private Coroutine _changeMaskCoroutine;
		private Coroutine _changeStateCoroutine;

		private void Awake()
		{
			var (color, intensity, intersectionIntensity, maskPower) = GetStateData(_barrier.IsCharged);

			SetColor(color.CalculateIntensityColor(intensity), color.CalculateIntensityColor(intersectionIntensity));
			SetMask(maskPower);

			_maskStateCoroutine = StartCoroutine(Idle());
		}

		private void OnEnable() =>
			_barrier.ChargeChanged += OnChargeChanged;

		private void OnDisable() =>
			_barrier.ChargeChanged -= OnChargeChanged;

		private void OnChargeChanged()
		{
			this.StopRoutine(_changeMaskCoroutine);
			this.StopRoutine(_maskStateCoroutine);
			this.StopRoutine(_changeStateCoroutine);

			var isCharged = _barrier.IsCharged;

			_changeStateCoroutine = StartCoroutine(ChangeState(isCharged, () => _maskStateCoroutine = StartCoroutine(Idle())));
		}

		private (Color, float, float, float) GetStateData(bool condition) =>
			condition is false
				? (_onColor, _intensityOnColor, _intersectionOnIntensity, _onMaskPower)
				: (_offColor, _intensityOffColor, _intersectionOffIntensity, _offMaskPower);

		private void SetColor(Color color, Color intersectionColor)
		{
			_meshRendererWall.material.SetColor(BorderColor, color);
			_meshRendererWall.material.SetColor(DotsColor, color);
			_meshRendererWall.material.SetColor(IntersectionColor, intersectionColor);

			foreach (var meshRenderer in _meshRenderersBase)
			{
				meshRenderer.material.SetColor(BaseColor, color);
			}
		}

		private Color GetColor() =>
			_meshRendererWall.material.GetColor(DotsColor);

		private void SetMask(float maskPower) =>
			_meshRendererWall.material.SetFloat(MaskPower, maskPower);

		private float GetMask() =>
			_meshRendererWall.material.GetFloat(MaskPower);

		private IEnumerator Idle()
		{
			var startMaskPower = GetMask();
			var currentMaskPower = startMaskPower;

			while (enabled)
			{
				var targetMaskPower = Random.Range(startMaskPower - _rangeMaskPower.Min, startMaskPower + _rangeMaskPower.Max);

				_changeMaskCoroutine = this.PlaySmoothChangeValue(
					(currentTime) => LerpMask(currentMaskPower, targetMaskPower, currentTime),
					_timeChangeMaskPowerOnIdle);

				yield return _changeMaskCoroutine;

				currentMaskPower = GetMask();
			}
		}

		private IEnumerator ChangeState(bool isCharged, Action endCallback)
		{
			var (color, intensity, intersectionIntensity, maskPower) = GetStateData(isCharged);

			_maskStateCoroutine = this.PlaySmoothChangeValue(
				(currentTime) =>
				{
					LerpColor(
						GetColor(),
						color.CalculateIntensityColor(intensity),
						color.CalculateIntensityColor(intersectionIntensity),
						currentTime);

					LerpMask(GetMask(), maskPower, currentTime);
				},
				_timeChangeMaskPower);

			yield return _maskStateCoroutine;

			endCallback?.Invoke();
		}

		private void LerpMask(float currentMaskPower, float targetMaskPower, float currentTime)
		{
			var resultMaskPower = Mathf.Lerp(currentMaskPower, targetMaskPower, currentTime);

			SetMask(resultMaskPower);
		}

		private void LerpColor(Color currentColor, Color targetColor, Color intersectionColor, float currentTime)
		{
			var resultMaskColor = Color.Lerp(currentColor, targetColor, currentTime);

			var resultIntersectionColor = Color.Lerp(currentColor, intersectionColor, currentTime);

			SetColor(resultMaskColor, resultIntersectionColor);
		}
	}
}