using System;
using System.Collections;
using CubeProject.Game;
using CubeProject.PlayableCube;
using LeadTools.Extensions;
using LeadTools.Other;
using LeadTools.StateMachine;
using Reflex.Attributes;
using Source.Scripts.Game.Level.Shield.States;
using UnityEngine;

namespace Source.Scripts.Game.Level.Shield
{
	public class ShieldView : MonoBehaviour
	{
		private const ShaderProperty FresnelPowerProperty = ShaderProperty._Fresnel_Power;
		private const ShaderProperty DisplacementAmountProperty = ShaderProperty._Displacement_Amount;
		
		[SerializeField] private MeshRenderer _renderer;
		
		private Vector2 _distanceRange;
		private Vector2 _fresnelPowerRange;
		private Vector2 _displacementAmountRange;
		private float _displacementAmountHide;
		private float _hideShowDuration;
		private IStateChangeable<ShieldStateMachine> _stateMachine;
		private Coroutine _viewCoroutine;
		private Coroutine _changeShieldVisible;
		private Func<Transform> _getCubeTransform;
		private Func<Transform> _getTargetTransform;
		private Func<IChargeable> _getChargeable;

		[Inject]
		private void Inject(Cube cube)
		{
			(_distanceRange, _fresnelPowerRange, _displacementAmountRange, _displacementAmountHide, _hideShowDuration)
				= cube.Component.Data.GetShieldData;
			_stateMachine = cube.Component.ShieldService.StateMachine;

			_getCubeTransform = () => cube.transform;
			_getTargetTransform = () => cube.Component.TriggerObserver.CurrentTarget;
			_getChargeable = () => cube.Component.TriggerObserver.Chargeable;
			
			_stateMachine.SubscribeTo<PlayState>(OnPlay);
			_stateMachine.SubscribeTo<StopState>(OnStop);

			_renderer.enabled = false;
			OnStop(true);
		}

		private void OnDisable()
		{
			_stateMachine.UnSubscribeTo<PlayState>(OnPlay);
			_stateMachine.UnSubscribeTo<StopState>(OnStop);
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

			_changeShieldVisible = StartCoroutine(ChangeShieldVisible(false));
		}

		private IEnumerator UpdateView()
		{
			var waitUpdate = new WaitForFixedUpdate();
			
			while (enabled)
			{
				if (_getTargetTransform() is not null && _getChargeable() is { IsCharged: true })
				{
					if (_renderer.enabled == false)
						yield return ChangeShieldVisible(true);
					
					UpdateShieldProperties();
				}
				else
				{
					if (_renderer.enabled)
						yield return ChangeShieldVisible(false);
				}
				
				yield return waitUpdate;
			}
		}

		private IEnumerator ChangeShieldVisible(bool isShow)
		{
			var targetValue = GetPropertyValueOnDistance(_displacementAmountRange, CalculateNormalDistance());
			UpdateProperty(DisplacementAmountProperty.GetCurrentName(), _displacementAmountHide);

			if (isShow)
				_renderer.enabled = true;
			
			yield return this.PlaySmoothChangeValue(
				(normalTime) =>
				{
					if (isShow)
						normalTime = 1 - normalTime;
					
					var value = Mathf.Lerp(targetValue, _displacementAmountHide, normalTime);
					UpdateProperty(DisplacementAmountProperty.GetCurrentName(), value);
				},
				_hideShowDuration);

			if (isShow == false)
				_renderer.enabled = false;
		}
		
		private void UpdateShieldProperties()
		{
			var normalDistance = CalculateNormalDistance();
			
			UpdateProperty(
				FresnelPowerProperty.GetCurrentName(), 
				GetPropertyValueOnDistance(_fresnelPowerRange, 1 - normalDistance));
			
			UpdateProperty(
				DisplacementAmountProperty.GetCurrentName(),
				GetPropertyValueOnDistance(_displacementAmountRange, normalDistance));
		}

		private float CalculateNormalDistance()
		{
			var targetTransform = _getTargetTransform();
			float distance;

			if (targetTransform is null)
				distance = _distanceRange.y;
			else
				distance = Vector3.Distance(_getCubeTransform().position, _getTargetTransform().position);
			
			float normalDistance;

			if (distance > _distanceRange.x)
				normalDistance = distance / _distanceRange.y;
			else 
				normalDistance = 1;

			return normalDistance;
		}
		
		private void UpdateProperty(string propertyName, float value) =>
			_renderer.material.SetFloat(propertyName, value);

		private float GetPropertyValueOnDistance(Vector2 range, float normalDistance) =>
			Mathf.Lerp(range.x, range.y, normalDistance);
	}
}