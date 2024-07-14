using System.Collections;
using System.Threading;
using CubeProject.Game.Level.Trigger;
using CubeProject.Game.Player;
using CubeProject.Game.Player.Shield.States;
using Cysharp.Threading.Tasks;
using LeadTools.Extensions;
using LeadTools.Other;
using LeadTools.StateMachine;
using Reflex.Attributes;
using UnityEngine;

namespace CubeProject.Game.Player.Shield
{
	public class ShieldView : MonoBehaviour
	{
		private readonly string _fresnelPowerProperty = ShaderProperty._Fresnel_Power.GetCurrentName();
		private readonly string _displacementAmountProperty = ShaderProperty._Displacement_Amount.GetCurrentName();

		[SerializeField] private MeshRenderer _renderer;

		private ShieldData _shieldData;
		private Coroutine _viewCoroutine;
		private Coroutine _changeShieldVisible;
		private Transform _cubeTransform;
		private TriggerDetector _triggerDetector;
		private IStateChangeable<ShieldStateMachine> _shieldStateChangeable;
		private CancellationTokenSource _cancellationTokenSource;

		[Inject]
		private void Inject(CubeComponent cubeComponent, IStateChangeable<ShieldStateMachine> shieldStateChangeable)
		{
			_shieldData = cubeComponent.Data.ShieldData;

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

		private async void OnPlay(bool isEntered)
		{
			if (isEntered == false)
				return;

			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = new CancellationTokenSource();

			await UniTask.Create(UpdateView, _cancellationTokenSource.Token);
		}

		private async void OnStop(bool isEntered)
		{
			if (isEntered == false)
				return;

			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = new CancellationTokenSource();
			
			await UniTask.Create(
				token => ChangeShieldVisible(token), 
				_cancellationTokenSource.Token);
		}

		private async UniTask UpdateView(CancellationToken cancellationToken)
		{
			while (_shieldStateChangeable.CurrentState == typeof(PlayState))
			{
				if (_triggerDetector.TryGetTargetTransform(out var targetTransform))
				{
					var normalDistance = CalculateNormalDistance(targetTransform);

					if (_renderer.enabled == false)
						await ChangeShieldVisible(cancellationToken, true, normalDistance);

					UpdateShieldProperties(normalDistance);
				}
				else
				{
					if (_renderer.enabled)
						await ChangeShieldVisible(cancellationToken);
				}

				await UniTask.WaitForFixedUpdate(cancellationToken);
			}
		}
		
		private async UniTask ChangeShieldVisible(CancellationToken cancellationToken, bool isShow = false, float normalDistance = 0)
		{
			if (normalDistance == 0)
				normalDistance = _shieldData.DistanceRange.y;
			
			var targetValue = GetPropertyValueOnDistance(_shieldData.FresnelPowerRange,
				normalDistance);

			UpdateProperty(_fresnelPowerProperty, _shieldData.FresnelPowerHide);

			if (isShow)
				_renderer.enabled = true;

			await MonoBehaviourExtension.SmoothChangeValue(
				(normalTime) =>
				{
					if (isShow)
						normalTime = 1 - normalTime;

					var value = Mathf.Lerp(targetValue, _shieldData.FresnelPowerHide, normalTime);
					UpdateProperty(_fresnelPowerProperty, value);
				},
				_shieldData.HideShowDuration,
				cancellationToken);

			if (isShow == false)
				_renderer.enabled = false;
		}

		private void UpdateShieldProperties(float normalDistance)
		{
			UpdateProperty(
				_fresnelPowerProperty,
				GetPropertyValueOnDistance(
					new Vector2(_shieldData.FresnelPowerRange.y, _shieldData.FresnelPowerRange.x),
					1 - normalDistance));

			UpdateProperty(
				_displacementAmountProperty,
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