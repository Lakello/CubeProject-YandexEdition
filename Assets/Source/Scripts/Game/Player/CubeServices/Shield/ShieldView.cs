using System;
using System.Collections;
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
		private IStateChangeable<ShieldStateMachine> _stateMachine;
		private Coroutine _viewCoroutine;
		private Func<Transform> _getCubeTransform;
		private Func<Transform> _getTargetTransform; 

		[Inject]
		private void Inject(Cube cube)
		{
			(_distanceRange, _fresnelPowerRange, _displacementAmountRange) = cube.Component.Data.GetShieldData;
			_stateMachine = cube.Component.ShieldService.StateMachine;

			_getCubeTransform = () => cube.transform;
			_getTargetTransform = () => cube.Component.TriggerObserver.CurrentTarget;
			
			_stateMachine.SubscribeTo<PlayState>(OnPlay);
			_stateMachine.SubscribeTo<StopState>(OnStop);
			
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
			
			_viewCoroutine = StartCoroutine(UpdateView());
		}

		private void OnStop(bool isEntered)
		{
			if (isEntered == false)
				return;
			
			this.StopRoutine(_viewCoroutine);

			_renderer.enabled = false;
		}

		private IEnumerator UpdateView()
		{
			var waitUpdate = new WaitForFixedUpdate();
			var waitTarget = new WaitUntil(() => _getTargetTransform != null);
			
			while (enabled)
			{
				if (_getTargetTransform() == null)
				{
					Debug.Log($"Target NULL");
					_renderer.enabled = false;
					yield return waitTarget;
				}
				else
				{
					Debug.Log($"UPDATE");
					if (_renderer.enabled == false)
						_renderer.enabled = true;
					
					UpdateShieldProperties();
				}
				
				yield return waitUpdate;
			}
		}

		private void UpdateShieldProperties()
		{
			var distance = Vector3.Distance(_getCubeTransform().position, _getTargetTransform().position);
			float normalDistance;

			if (distance > _distanceRange.x)
				normalDistance = 1 - distance / _distanceRange.y;
			else 
				normalDistance = 1;
				
			UpdateProperty(_fresnelPowerRange, normalDistance, FresnelPowerProperty.GetCurrentName());
			UpdateProperty(_displacementAmountRange, normalDistance, DisplacementAmountProperty.GetCurrentName());
		}

		private void UpdateProperty(Vector2 range, float normalDistance, string propertyName)
		{
			var value = Mathf.Lerp(range.x, range.y, normalDistance);
			
			_renderer.material.SetFloat(propertyName, value);
		}
	}
}