using System.Collections;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using LeadTools.Other;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Source.Scripts.Game.Level.Shield.States;
using Source.Scripts.Game.Level.Trigger;
using UnityEngine;

namespace Source.Scripts.Game.Level.Shield
{
	public class ShieldView : MonoBehaviour
	{
		private const ShaderProperty FresnelPowerProperty = ShaderProperty._Fresnel_Power;
		private const ShaderProperty DisplacementAmountProperty = ShaderProperty._Displacement_Amount;

		[SerializeField] private MeshRenderer _renderer;

		private ShieldData _shieldData;
		private Coroutine _viewCoroutine;
		private Coroutine _changeShieldVisible;
		private Transform _cubeTransform;
		private TriggerDetector _triggerDetector;
		private IStateChangeable<ShieldStateMachine> _shieldStateChangeable;

		[Inject]
		private void Inject(CubeComponent cubeComponent, IStateChangeable<ShieldStateMachine> shieldStateChangeable)
		{
			_shieldData = cubeComponent.Data.GetShieldData;

			_cubeTransform = cubeComponent.transform;
			_triggerDetector = cubeComponent.TriggerDetector;

			_shieldStateChangeable = shieldStateChangeable;

			_shieldStateChangeable.SubscribeTo<PlayState>(OnPlay);
			_shieldStateChangeable.SubscribeTo<StopState>(OnStop);

			_renderer.enabled = false;
			OnStop(true);
		}

		private void OnDisable()
		{
			_shieldStateChangeable.UnSubscribeTo<PlayState>(OnPlay);
			_shieldStateChangeable.UnSubscribeTo<StopState>(OnStop);
		}

		private void OnPlay(bool isEntered)
		{
			if (isEntered == false)
				return;

			this.StopRoutine(_changeShieldVisible);

			_viewCoroutine = StartCoroutine(UpdateView());
		}

		private void OnStop(bool isEntered)
		{
			if (isEntered == false)
				return;

			this.StopRoutine(_viewCoroutine);
			this.StopRoutine(_changeShieldVisible);

			_changeShieldVisible = StartCoroutine(ChangeShieldVisible(false, _shieldData.DistanceRange.y));
		}

		private IEnumerator UpdateView()
		{
			var waitUpdate = new WaitForFixedUpdate();

			while (enabled)
			{
				if (_triggerDetector.TryGetTargetTransform(out var targetTransform))
				{
					var normalDistance = CalculateNormalDistance(targetTransform);

					if (_renderer.enabled == false)
						yield return ChangeShieldVisible(true, normalDistance);

					UpdateShieldProperties(normalDistance);
				}
				else
				{
					if (_renderer.enabled)
						yield return ChangeShieldVisible(false, _shieldData.DistanceRange.y);
				}

				yield return waitUpdate;
			}
		}

		private IEnumerator ChangeShieldVisible(bool isShow, float normalDistance)
		{
			var targetValue = GetPropertyValueOnDistance(_shieldData.DisplacementAmountRange, normalDistance);
			UpdateProperty(DisplacementAmountProperty.GetCurrentName(), _shieldData.DisplacementAmountHide);

			if (isShow)
				_renderer.enabled = true;

			yield return this.PlaySmoothChangeValue(
				(normalTime) =>
				{
					if (isShow)
						normalTime = 1 - normalTime;

					var value = Mathf.Lerp(targetValue, _shieldData.DisplacementAmountHide, normalTime);
					UpdateProperty(DisplacementAmountProperty.GetCurrentName(), value);
				},
				_shieldData.HideShowDuration);

			if (isShow == false)
				_renderer.enabled = false;
		}

		private void UpdateShieldProperties(float normalDistance)
		{
			UpdateProperty(
				FresnelPowerProperty.GetCurrentName(),
				GetPropertyValueOnDistance(_shieldData.FresnelPowerRange, 1 - normalDistance));

			UpdateProperty(
				DisplacementAmountProperty.GetCurrentName(),
				GetPropertyValueOnDistance(_shieldData.DisplacementAmountRange, normalDistance));
		}

		private float CalculateNormalDistance(Transform targetTransform)
		{
			float distance = targetTransform is null
				? _shieldData.DistanceRange.y
				: Vector3.Distance(_cubeTransform.position, targetTransform.position);

			return distance > _shieldData.DistanceRange.x
				? distance / _shieldData.DistanceRange.y
				: 1;
		}

		private void UpdateProperty(string propertyName, float value) =>
			_renderer.material.SetFloat(propertyName, value);

		private float GetPropertyValueOnDistance(Vector2 range, float normalDistance) =>
			Mathf.Lerp(range.x, range.y, normalDistance);
	}
}